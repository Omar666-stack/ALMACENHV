using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApuntesController : BaseController
    {
        public ApuntesController(AlmacenContext context, ILogger<ApuntesController> logger)
            : base(context, logger)
        {
        }

        // GET: api/Apuntes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Apunte>>> GetApuntes()
        {
            return await HandleDbOperationList<Apunte>(
                async () => await _context.Apuntes.Include(a => a.Usuario).ToListAsync(),
                "Error retrieving apuntes");
        }

        // GET: api/Apuntes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Apunte>> GetApunte(int id)
        {
            return await HandleDbOperation<Apunte>(
                async () => await _context.Apuntes
                    .Include(a => a.Usuario)
                    .FirstOrDefaultAsync(a => a.ApunteID == id),
                $"Error retrieving apunte with ID {id}");
        }

        // PUT: api/Apuntes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApunte(int id, Apunte apunte)
        {
            if (id != apunte.ApunteID)
            {
                return BadRequest();
            }

            _context.Entry(apunte).State = EntityState.Modified;

            return await HandleDbUpdate<Apunte>(
                apunte,
                async () => await _context.SaveChangesAsync(),
                $"Error updating apunte with ID {id}");
        }

        // POST: api/Apuntes
        [HttpPost]
        public async Task<ActionResult<Apunte>> PostApunte(Apunte apunte)
        {
            _context.Apuntes.Add(apunte);
            
            return await HandleDbCreate<Apunte>(
                apunte,
                async () => await _context.SaveChangesAsync(),
                "Error creating apunte");
        }

        // DELETE: api/Apuntes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApunte(int id)
        {
            return await HandleDbDelete<Apunte>(
                async () => await _context.Apuntes.FindAsync(id),
                async (apunte) => {
                    _context.Apuntes.Remove(apunte);
                    await _context.SaveChangesAsync();
                },
                $"Error deleting apunte with ID {id}");
        }
    }
}
