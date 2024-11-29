using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbicacionesController : BaseController
    {
        public UbicacionesController(AlmacenContext context, ILogger<UbicacionesController> logger)
            : base(context, logger)
        {
        }

        // GET: api/Ubicaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ubicacion>>> GetUbicaciones()
        {
            var result = await HandleDbOperationList<Ubicacion>(
                async () => await _context.Ubicaciones
                    .Include(u => u.Seccion)
                    .Include(u => u.RegistroEntradas)
                    .Include(u => u.RegistroSalidas)
                    .ToListAsync(),
                "Error retrieving ubicaciones"
            );

            if (result.Result is ObjectResult objResult && objResult.Value is List<Ubicacion> ubicaciones)
            {
                return Ok(ubicaciones.AsEnumerable());
            }

            return result;
        }

        // GET: api/Ubicaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ubicacion>> GetUbicacion(int id)
        {
            return await HandleDbOperation<Ubicacion>(
                async () => await _context.Ubicaciones
                    .Include(u => u.Seccion)
                    .Include(u => u.RegistroEntradas)
                    .Include(u => u.RegistroSalidas)
                    .FirstOrDefaultAsync(u => u.UbicacionID == id),
                $"Error retrieving ubicacion with ID {id}"
            );
        }

        // PUT: api/Ubicaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUbicacion(int id, Ubicacion ubicacion)
        {
            if (id != ubicacion.UbicacionID)
            {
                return BadRequest("El ID de la ubicación no coincide");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validar que la sección existe
            var seccionExiste = await _context.Secciones.AnyAsync(s => s.SeccionID == ubicacion.SeccionID);
            if (!seccionExiste)
            {
                return BadRequest("La sección especificada no existe");
            }

            // Validar que el código no esté duplicado (excluyendo la ubicación actual)
            var codigoExiste = await _context.Ubicaciones
                .AnyAsync(u => u.Codigo == ubicacion.Codigo && u.UbicacionID != id);
            if (codigoExiste)
            {
                return BadRequest("Ya existe otra ubicación con este código");
            }

            return await HandleDbUpdate(
                ubicacion,
                async () =>
                {
                    _context.Entry(ubicacion).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                },
                "Error al actualizar la ubicación"
            );
        }

        // POST: api/Ubicaciones
        [HttpPost]
        public async Task<ActionResult<Ubicacion>> PostUbicacion(Ubicacion ubicacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validar que la sección existe
            var seccionExiste = await _context.Secciones.AnyAsync(s => s.SeccionID == ubicacion.SeccionID);
            if (!seccionExiste)
            {
                return BadRequest("La sección especificada no existe");
            }

            // Validar que el código no esté duplicado
            var codigoExiste = await _context.Ubicaciones.AnyAsync(u => u.Codigo == ubicacion.Codigo);
            if (codigoExiste)
            {
                return BadRequest("Ya existe una ubicación con este código");
            }

            return await HandleDbCreate(
                ubicacion,
                async () =>
                {
                    _context.Ubicaciones.Add(ubicacion);
                    await _context.SaveChangesAsync();
                },
                "Error al crear la ubicación"
            );
        }

        // DELETE: api/Ubicaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUbicacion(int id)
        {
            return await HandleDbDelete<Ubicacion>(
                async () => await _context.Ubicaciones.FindAsync(id),
                async (ubicacion) =>
                {
                    _context.Ubicaciones.Remove(ubicacion);
                    await _context.SaveChangesAsync();
                },
                $"Error deleting ubicacion with ID {id}"
            );
        }
    }
}
