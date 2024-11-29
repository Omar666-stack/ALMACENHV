using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UbicacionesController : BaseController
    {
        private readonly IMemoryCache _cache;
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> _locks = new();
        private const string CACHE_KEY_ALL_UBICACIONES = "AllUbicaciones";
        private const string CACHE_KEY_UBICACION = "Ubicacion_";
        private readonly TimeSpan CACHE_DURATION = TimeSpan.FromMinutes(15);

        public UbicacionesController(AlmacenContext context, ILogger<UbicacionesController> logger, IMemoryCache cache)
            : base(context, logger)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        // GET: api/Ubicaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ubicacion>>> GetUbicaciones()
        {
            try
            {
                // Intentar obtener ubicaciones del caché
                if (_cache.TryGetValue(CACHE_KEY_ALL_UBICACIONES, out IEnumerable<Ubicacion> cachedUbicaciones))
                {
                    return Ok(cachedUbicaciones);
                }

                // Si no está en caché, obtener de la base de datos
                var ubicaciones = await _context.Ubicaciones
                    .AsNoTracking()
                    .Include(u => u.Seccion)
                    .AsSplitQuery()
                    .ToListAsync();

                // Guardar en caché
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(CACHE_DURATION)
                    .SetPriority(CacheItemPriority.Normal);

                _cache.Set(CACHE_KEY_ALL_UBICACIONES, ubicaciones, cacheEntryOptions);

                return Ok(ubicaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ubicaciones");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/Ubicaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ubicacion>> GetUbicacion(int id)
        {
            try
            {
                string cacheKey = $"{CACHE_KEY_UBICACION}{id}";

                // Intentar obtener ubicación del caché
                if (_cache.TryGetValue(cacheKey, out Ubicacion cachedUbicacion))
                {
                    return Ok(cachedUbicacion);
                }

                // Obtener o crear el semáforo para este ID
                var lockObj = _locks.GetOrAdd(id, _ => new SemaphoreSlim(1, 1));
                await lockObj.WaitAsync();

                try
                {
                    // Verificar caché nuevamente después de obtener el lock
                    if (_cache.TryGetValue(cacheKey, out cachedUbicacion))
                    {
                        return Ok(cachedUbicacion);
                    }

                    var ubicacion = await _context.Ubicaciones
                        .AsNoTracking()
                        .Include(u => u.Seccion)
                        .AsSplitQuery()
                        .FirstOrDefaultAsync(u => u.UbicacionID == id);

                    if (ubicacion == null)
                    {
                        return NotFound($"Ubicación con ID {id} no encontrada");
                    }

                    // Guardar en caché
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(CACHE_DURATION)
                        .SetPriority(CacheItemPriority.Normal);

                    _cache.Set(cacheKey, ubicacion, cacheEntryOptions);

                    return Ok(ubicacion);
                }
                finally
                {
                    lockObj.Release();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener ubicación con ID {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/Ubicaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUbicacion(int id, Ubicacion ubicacion)
        {
            if (id != ubicacion.UbicacionID)
            {
                return BadRequest("El ID de la ubicación no coincide");
            }

            try
            {
                // Validar que la sección existe
                var seccionExiste = await _context.Secciones.AnyAsync(s => s.SeccionID == ubicacion.SeccionID);
                if (!seccionExiste)
                {
                    return BadRequest("La sección especificada no existe");
                }

                // Validar que el código no esté duplicado
                var codigoExiste = await _context.Ubicaciones
                    .AsNoTracking()
                    .AnyAsync(u => u.Codigo == ubicacion.Codigo && u.UbicacionID != id);

                if (codigoExiste)
                {
                    return BadRequest("Ya existe otra ubicación con este código");
                }

                _context.Entry(ubicacion).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Invalidar caché
                _cache.Remove(CACHE_KEY_ALL_UBICACIONES);
                _cache.Remove($"{CACHE_KEY_UBICACION}{id}");

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UbicacionExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar ubicación con ID {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // POST: api/Ubicaciones
        [HttpPost]
        public async Task<ActionResult<Ubicacion>> PostUbicacion(Ubicacion ubicacion)
        {
            try
            {
                // Validar que la sección existe
                var seccionExiste = await _context.Secciones.AnyAsync(s => s.SeccionID == ubicacion.SeccionID);
                if (!seccionExiste)
                {
                    return BadRequest("La sección especificada no existe");
                }

                // Validar que el código no esté duplicado
                var codigoExiste = await _context.Ubicaciones
                    .AsNoTracking()
                    .AnyAsync(u => u.Codigo == ubicacion.Codigo);

                if (codigoExiste)
                {
                    return BadRequest("Ya existe una ubicación con este código");
                }

                _context.Ubicaciones.Add(ubicacion);
                await _context.SaveChangesAsync();

                // Invalidar caché de todas las ubicaciones
                _cache.Remove(CACHE_KEY_ALL_UBICACIONES);

                return CreatedAtAction(nameof(GetUbicacion), new { id = ubicacion.UbicacionID }, ubicacion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear ubicación");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // DELETE: api/Ubicaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUbicacion(int id)
        {
            try
            {
                var ubicacion = await _context.Ubicaciones.FindAsync(id);
                if (ubicacion == null)
                {
                    return NotFound();
                }

                _context.Ubicaciones.Remove(ubicacion);
                await _context.SaveChangesAsync();

                // Invalidar caché
                _cache.Remove(CACHE_KEY_ALL_UBICACIONES);
                _cache.Remove($"{CACHE_KEY_UBICACION}{id}");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar ubicación con ID {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        private async Task<bool> UbicacionExists(int id)
        {
            return await _context.Ubicaciones.AnyAsync(e => e.UbicacionID == id);
        }
    }
}
