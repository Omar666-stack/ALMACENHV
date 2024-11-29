using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            return await HandleDbOperationList(
                async () => await _context.Secciones
                    .Include(s => s.Ubicaciones)
                    .ToListAsync()
            );
        }

        // GET: api/Secciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Seccion>> GetSeccion(int id)
        {
            return await HandleDbOperation(
                async () => await _context.Secciones
                    .Include(s => s.Ubicaciones)
                    .FirstOrDefaultAsync(s => s.SeccionID == id)
            );
        }

        // POST: api/Secciones
        [HttpPost]
        public async Task<ActionResult<Seccion>> PostSeccion(Seccion seccion)
        {
            return await HandleDbCreate(
                seccion,
                async () =>
                {
                    _context.Secciones.Add(seccion);
                    await _context.SaveChangesAsync();
                }
            );
        }

        // PUT: api/Secciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeccion(int id, Seccion seccion)
        {
            if (id != seccion.SeccionID)
            {
                return BadRequest();
            }

            return await HandleDbUpdate(
                seccion,
                async () =>
                {
                    _context.Entry(seccion).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            );
        }

        // DELETE: api/Secciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeccion(int id)
        {
            return await HandleDbDelete(
                async () => await _context.Secciones.FindAsync(id),
                async (seccion) =>
                {
                    _context.Secciones.Remove(seccion);
                    await _context.SaveChangesAsync();
                }
            );
        }
    }
}
