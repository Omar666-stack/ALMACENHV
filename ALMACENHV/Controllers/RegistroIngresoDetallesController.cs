using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ALMACENHV.Models; // Asegúrate de que este espacio de nombres esté incluido

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroIngresoDetallesController : ControllerBase
    {
        private readonly TuDbContext _context;

        public RegistroIngresoDetallesController(TuDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroIngresoDetalle>>> GetRegistroIngresoDetalles()
        {
            return await _context.RegistroIngresoDetalles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroIngresoDetalle>> GetRegistroIngresoDetalle(int id)
        {
            var registroIngresoDetalle = await _context.RegistroIngresoDetalles.FindAsync(id);

            if (registroIngresoDetalle == null)
            {
                return NotFound();
            }

            return registroIngresoDetalle;
        }

        [HttpPost]
        public async Task<ActionResult<RegistroIngresoDetalle>> PostRegistroIngresoDetalle(RegistroIngresoDetalle registroIngresoDetalle)
        {
            _context.RegistroIngresoDetalles.Add(registroIngresoDetalle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegistroIngresoDetalle", new { id = registroIngresoDetalle.RegistroIngresoDetalleID }, registroIngresoDetalle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroIngresoDetalle(int id, RegistroIngresoDetalle registroIngresoDetalle)
        {
            if (id != registroIngresoDetalle.RegistroIngresoDetalleID)
            {
                return BadRequest();
            }

            _context.Entry(registroIngresoDetalle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroIngresoDetalleExists(id))
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
        public async Task<IActionResult> DeleteRegistroIngresoDetalle(int id)
        {
            var registroIngresoDetalle = await _context.RegistroIngresoDetalles.FindAsync(id);
            if (registroIngresoDetalle == null)
            {
                return NotFound();
            }

            _context.RegistroIngresoDetalles.Remove(registroIngresoDetalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroIngresoDetalleExists(int id)
        {
            return _context.RegistroIngresoDetalles.Any(e => e.RegistroIngresoDetalleID == id);
        }
    }
}