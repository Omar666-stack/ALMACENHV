using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ALMACENHV.Models; // Asegúrate de que este espacio de nombres esté incluido

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroSalidaDetallesController : ControllerBase
    {
        private readonly TuDbContext _context;

        public RegistroSalidaDetallesController(TuDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroSalidaDetalle>>> GetRegistroSalidaDetalles()
        {
            return await _context.RegistroSalidaDetalles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroSalidaDetalle>> GetRegistroSalidaDetalle(int id)
        {
            var registroSalidaDetalle = await _context.RegistroSalidaDetalles.FindAsync(id);

            if (registroSalidaDetalle == null)
            {
                return NotFound();
            }

            return registroSalidaDetalle;
        }

        [HttpPost]
        public async Task<ActionResult<RegistroSalidaDetalle>> PostRegistroSalidaDetalle(RegistroSalidaDetalle registroSalidaDetalle)
        {
            _context.RegistroSalidaDetalles.Add(registroSalidaDetalle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegistroSalidaDetalle", new { id = registroSalidaDetalle.RegistroSalidaDetalleID }, registroSalidaDetalle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroSalidaDetalle(int id, RegistroSalidaDetalle registroSalidaDetalle)
        {
            if (id != registroSalidaDetalle.RegistroSalidaDetalleID)
            {
                return BadRequest();
            }

            _context.Entry(registroSalidaDetalle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroSalidaDetalleExists(id))
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
        public async Task<IActionResult> DeleteRegistroSalidaDetalle(int id)
        {
            var registroSalidaDetalle = await _context.RegistroSalidaDetalles.FindAsync(id);
            if (registroSalidaDetalle == null)
            {
                return NotFound();
            }

            _context.RegistroSalidaDetalles.Remove(registroSalidaDetalle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroSalidaDetalleExists(int id)
        {
            return _context.RegistroSalidaDetalles.Any(e => e.RegistroSalidaDetalleID == id);
        }
    }
}