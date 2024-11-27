using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ALMACENHV.Models; // Asegúrate de que este espacio de nombres esté incluido

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroSalidasController : ControllerBase
    {
        private readonly TuDbContext _context;

        public RegistroSalidasController(TuDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroSalida>>> GetRegistroSalidas()
        {
            return await _context.RegistroSalidas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroSalida>> GetRegistroSalida(int id)
        {
            var registroSalida = await _context.RegistroSalidas.FindAsync(id);

            if (registroSalida == null)
            {
                return NotFound();
            }

            return registroSalida;
        }

        [HttpPost]
        public async Task<ActionResult<RegistroSalida>> PostRegistroSalida(RegistroSalida registroSalida)
        {
            _context.RegistroSalidas.Add(registroSalida);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegistroSalida", new { id = registroSalida.RegistroSalidaID }, registroSalida);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroSalida(int id, RegistroSalida registroSalida)
        {
            if (id != registroSalida.RegistroSalidaID)
            {
                return BadRequest();
            }

            _context.Entry(registroSalida).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroSalidaExists(id))
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
        public async Task<IActionResult> DeleteRegistroSalida(int id)
        {
            var registroSalida = await _context.RegistroSalidas.FindAsync(id);
            if (registroSalida == null)
            {
                return NotFound();
            }

            _context.RegistroSalidas.Remove(registroSalida);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroSalidaExists(int id)
        {
            return _context.RegistroSalidas.Any(e => e.RegistroSalidaID == id);
        }
    }
}