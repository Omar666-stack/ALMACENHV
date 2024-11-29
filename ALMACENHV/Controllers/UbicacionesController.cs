using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UbicacionesController : BaseController
    {
        private readonly IMemoryCache _cache;
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> _locks = new();
        private const string CACHE_KEY_ALL_UBICACIONES = "AllUbicaciones";
        private const string CACHE_KEY_UBICACION = "Ubicacion_";
        private readonly TimeSpan CACHE_DURATION = TimeSpan.FromMinutes(15);
        private new readonly AlmacenContext _context;
        private new readonly ILogger<UbicacionesController> _logger;

        public UbicacionesController(
            AlmacenContext context,
            ILogger<UbicacionesController> logger,
            IMemoryCache cache) : base(context, logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ubicacion>>> GetUbicaciones([FromQuery] bool includeInactive = false)
        {
            return await HandleDbOperationList(
                async () =>
                {
                    string cacheKey = $"{CACHE_KEY_ALL_UBICACIONES}_{includeInactive}";
                    if (!_cache.TryGetValue(cacheKey, out List<Ubicacion> ubicaciones))
                    {
                        var query = _context.Ubicaciones
                            .Include(u => u.Seccion)
                            .Include(u => u.Productos)
                            .AsNoTracking();

                        if (!includeInactive)
                        {
                            query = query.Where(u => u.Activo);
                        }

                        ubicaciones = await query.ToListAsync();
                        _cache.Set(cacheKey, ubicaciones, CACHE_DURATION);
                    }
                    return ubicaciones;
                },
                "Error al obtener la lista de ubicaciones");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ubicacion>> GetUbicacion(int id)
        {
            string cacheKey = $"{CACHE_KEY_UBICACION}{id}";
            
            return await HandleDbOperation(
                async () =>
                {
                    if (!_cache.TryGetValue(cacheKey, out Ubicacion ubicacion))
                    {
                        ubicacion = await _context.Ubicaciones
                            .Include(u => u.Seccion)
                            .Include(u => u.Productos)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.UbicacionID == id);

                        if (ubicacion != null)
                        {
                            _cache.Set(cacheKey, ubicacion, CACHE_DURATION);
                        }
                    }
                    return ubicacion;
                },
                $"Error al obtener la ubicación con ID {id}");
        }

        [HttpGet("seccion/{seccionId}")]
        public async Task<ActionResult<IEnumerable<Ubicacion>>> GetUbicacionesPorSeccion(int seccionId)
        {
            return await HandleDbOperationList(
                async () =>
                {
                    var ubicaciones = await _context.Ubicaciones
                        .Include(u => u.Seccion)
                        .Include(u => u.Productos)
                        .Where(u => u.SeccionID == seccionId && u.Activo)
                        .AsNoTracking()
                        .ToListAsync();
                    return ubicaciones;
                },
                $"Error al obtener ubicaciones de la sección {seccionId}");
        }

        [HttpGet("disponibles")]
        public async Task<ActionResult<IEnumerable<Ubicacion>>> GetUbicacionesDisponibles()
        {
            return await HandleDbOperationList(
                async () =>
                {
                    var ubicaciones = await _context.Ubicaciones
                        .Include(u => u.Seccion)
                        .Include(u => u.Productos)
                        .Where(u => u.Activo)
                        .AsNoTracking()
                        .ToListAsync();

                    return ubicaciones.Where(u => u.EspacioDisponible > 0).ToList();
                },
                "Error al obtener ubicaciones disponibles");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUbicacion(int id, Ubicacion ubicacion)
        {
            if (id != ubicacion.UbicacionID)
            {
                return BadRequest("El ID de la ubicación no coincide con el ID de la ruta");
            }

            return await HandleDbUpdate(ubicacion, async () =>
            {
                ubicacion.FechaModificacion = DateTime.UtcNow;
                _context.Entry(ubicacion).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                InvalidateUbicacionCache(id);
            });
        }

        [HttpPost]
        public async Task<ActionResult<Ubicacion>> PostUbicacion(Ubicacion ubicacion)
        {
            return await HandleDbCreate(ubicacion, async () =>
            {
                ubicacion.FechaCreacion = DateTime.UtcNow;
                _context.Ubicaciones.Add(ubicacion);
                await _context.SaveChangesAsync();
                InvalidateUbicacionCache();
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUbicacion(int id)
        {
            var ubicacion = await _context.Ubicaciones.FindAsync(id);
            if (ubicacion == null)
            {
                return NotFound();
            }

            return await HandleDbDelete(ubicacion, async () =>
            {
                ubicacion.Activo = false;
                ubicacion.FechaModificacion = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                InvalidateUbicacionCache(id);
            });
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstadoUbicacion(int id, [FromBody] bool nuevoEstado)
        {
            var ubicacion = await _context.Ubicaciones
                .Include(u => u.Productos)
                .FirstOrDefaultAsync(u => u.UbicacionID == id);

            if (ubicacion == null)
            {
                return NotFound();
            }

            if (!nuevoEstado && ubicacion.Productos.Any())
            {
                return BadRequest("No se puede desactivar una ubicación que tiene productos almacenados");
            }

            return await HandleDbUpdate(ubicacion, async () =>
            {
                ubicacion.Activo = nuevoEstado;
                ubicacion.FechaModificacion = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                InvalidateUbicacionCache(id);
            });
        }

        private void InvalidateUbicacionCache(int? ubicacionId = null)
        {
            _cache.Remove(CACHE_KEY_ALL_UBICACIONES + "_true");
            _cache.Remove(CACHE_KEY_ALL_UBICACIONES + "_false");
            if (ubicacionId.HasValue)
            {
                _cache.Remove($"{CACHE_KEY_UBICACION}{ubicacionId}");
            }
        }
    }
}
