using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ALMACENHV.Models; // Asegúrate de que este espacio de nombres esté incluido

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroEntradasController : ControllerBase
    {
        private readonly TuDbContext _context;

        public RegistroEntradasController(TuDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroEntrada>>> GetRegistroEntradas()
        {
            return await _context.RegistroEntradas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroEntrada>> GetRegistroEntrada(int id)
        {
            var registroEntrada = await _context.RegistroEntradas.FindAsync(id);

            if (registroEntrada == null)
            {
                return NotFound();
            }

            return registroEntrada;
        }

        [HttpPost]
        public async Task<ActionResult<RegistroEntrada>> PostRegistroEntrada(RegistroEntrada registroEntrada)
        {
            _context.RegistroEntradas.Add(registroEntrada);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegistroEntrada", new { id = registroEntrada.RegistroEntradaID }, registroEntrada);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroEntrada(int id, RegistroEntrada registroEntrada)
        {
            if (id != registroEntrada.RegistroEntradaID)
            {
                return BadRequest();
            }

            _context.Entry(registroEntrada).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroEntradaExists(id))
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
        public async Task<IActionResult> DeleteRegistroEntrada(int id)
        {
            var registroEntrada = await _context.RegistroEntradas.FindAsync(id);
            if (registroEntrada == null)
            {
                return NotFound();
            }

            _context.RegistroEntradas.Remove(registroEntrada);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroEntradaExists(int id)
        {
            return _context.RegistroEntradas.Any(e => e.RegistroEntradaID == id);
        }
    }
}