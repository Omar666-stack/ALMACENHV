using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ALMACENHV.Models; // Asegúrate de que este espacio de nombres esté incluido

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeccionesController : ControllerBase
    {
        private readonly TuDbContext _context;

        public SeccionesController(TuDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seccion>>> GetSecciones()
        {
            return await _context.Secciones.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Seccion>> GetSeccion(int id)
        {
            var seccion = await _context.Secciones.FindAsync(id);

            if (seccion == null)
            {
                return NotFound();
            }

            return seccion;
        }

        [HttpPost]
        public async Task<ActionResult<Seccion>> PostSeccion(Seccion seccion)
        {
            _context.Secciones.Add(seccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeccion", new { id = seccion.SeccionID }, seccion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeccion(int id, Seccion seccion)
        {
            if (id != seccion.SeccionID)
            {
                return BadRequest();
            }

            _context.Entry(seccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeccionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeccion(int id)
        {
            var seccion = await _context.Secciones.FindAsync(id);
            if (seccion == null)
            {
                return NotFound();
            }

            _context.Secciones.Remove(seccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeccionExists(int id)
        {
            return _context.Secciones.Any(e => e.SeccionID == id);
        }
    }
}