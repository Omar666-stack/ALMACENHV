using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ALMACENHV.Models; // Asegúrate de que este espacio de nombres esté incluido

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApuntesController : ControllerBase
    {
        private readonly TuDbContext _context;

        public ApuntesController(TuDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Apunte>>> GetApuntes()
        {
            return await _context.Apuntes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Apunte>> GetApunte(int id)
        {
            var apunte = await _context.Apuntes.FindAsync(id);

            if (apunte == null)
            {
                return NotFound();
            }

            return apunte;
        }

        [HttpPost]
        public async Task<ActionResult<Apunte>> PostApunte(Apunte apunte)
        {
            _context.Apuntes.Add(apunte);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApunte", new { id = apunte.ApunteID }, apunte);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutApunte(int id, Apunte apunte)
        {
            if (id != apunte.ApunteID)
            {
                return BadRequest();
            }

            _context.Entry(apunte).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApunteExists(id))
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
        public async Task<IActionResult> DeleteApunte(int id)
        {
            var apunte = await _context.Apuntes.FindAsync(id);
            if (apunte == null)
            {
                return NotFound();
            }

            _context.Apuntes.Remove(apunte);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApunteExists(int id)
        {
            return _context.Apuntes.Any(e => e.ApunteID == id);
        }
    }
}