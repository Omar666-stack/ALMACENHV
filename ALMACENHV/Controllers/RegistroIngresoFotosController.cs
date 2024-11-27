using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ALMACENHV.Models; // Asegúrate de que este espacio de nombres esté incluido

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroIngresoFotosController : ControllerBase
    {
        private readonly TuDbContext _context;

        public RegistroIngresoFotosController(TuDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroIngresoFoto>>> GetRegistroIngresoFotos()
        {
            return await _context.RegistroIngresoFotos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroIngresoFoto>> GetRegistroIngresoFoto(int id)
        {
            var registroIngresoFoto = await _context.RegistroIngresoFotos.FindAsync(id);

            if (registroIngresoFoto == null)
            {
                return NotFound();
            }

            return registroIngresoFoto;
        }

        [HttpPost]
        public async Task<ActionResult<RegistroIngresoFoto>> PostRegistroIngresoFoto(RegistroIngresoFoto registroIngresoFoto)
        {
            _context.RegistroIngresoFotos.Add(registroIngresoFoto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegistroIngresoFoto", new { id = registroIngresoFoto.RegistroIngresoFotoID }, registroIngresoFoto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroIngresoFoto(int id, RegistroIngresoFoto registroIngresoFoto)
        {
            if (id != registroIngresoFoto.RegistroIngresoFotoID)
            {
                return BadRequest();
            }

            _context.Entry(registroIngresoFoto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroIngresoFotoExists(id))
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
        public async Task<IActionResult> DeleteRegistroIngresoFoto(int id)
        {
            var registroIngresoFoto = await _context.RegistroIngresoFotos.FindAsync(id);
            if (registroIngresoFoto == null)
            {
                return NotFound();
            }

            _context.RegistroIngresoFotos.Remove(registroIngresoFoto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroIngresoFotoExists(int id)
        {
            return _context.RegistroIngresoFotos.Any(e => e.RegistroIngresoFotoID == id);
        }
    }
}