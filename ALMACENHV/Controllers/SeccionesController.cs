using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;
using Microsoft.AspNetCore.Authorization;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SeccionesController : BaseController
    {
        public SeccionesController(AlmacenContext context, ILogger<SeccionesController> logger)
            : base(context, logger)
        {
        }

        // GET: api/Secciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seccion>>> GetSecciones()
        {
            return await HandleDbOperationList(async () =>
                await _context.Secciones
                    .Include(s => s.Ubicaciones)
                    .Where(s => s.Activo)
                    .ToListAsync());
        }

        // GET: api/Secciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Seccion>> GetSeccion(int id)
        {
            return await HandleDbOperation(async () =>
                await _context.Secciones
                    .Include(s => s.Ubicaciones)
                    .FirstOrDefaultAsync(s => s.SeccionID == id));
        }

        // PUT: api/Secciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeccion(int id, Seccion seccion)
        {
            if (id != seccion.SeccionID)
            {
                return BadRequest();
            }

            return await HandleDbUpdate(seccion, async () =>
            {
                seccion.FechaModificacion = DateTime.UtcNow;
                _context.Entry(seccion).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            });
        }

        // POST: api/Secciones
        [HttpPost]
        public async Task<ActionResult<Seccion>> PostSeccion(Seccion seccion)
        {
            return await HandleDbCreate(seccion, async () =>
            {
                seccion.FechaCreacion = DateTime.UtcNow;
                _context.Secciones.Add(seccion);
                await _context.SaveChangesAsync();
            });
        }

        // DELETE: api/Secciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeccion(int id)
        {
            var seccion = await _context.Secciones.FindAsync(id);
            if (seccion == null)
            {
                return NotFound();
            }

            return await HandleDbDelete(seccion, async () =>
            {
                seccion.Activo = false;
                seccion.FechaModificacion = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            });
        }

        private bool SeccionExists(int id)
        {
            return _context.Secciones.Any(e => e.SeccionID == id);
        }
    }
}
