using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroEntradasController : BaseController
    {
        public RegistroEntradasController(AlmacenContext context, ILogger<RegistroEntradasController> logger)
            : base(context, logger)
        {
        }

        // GET: api/RegistroEntradas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroEntrada>>> GetRegistroEntradas()
        {
            return await HandleDbOperationList<RegistroEntrada>(
                async () => await _context.RegistroEntradas
                    .Include(r => r.Usuario)
                    .Include(r => r.Proveedor)
                    .ToListAsync(),
                "Error retrieving registro entradas"
            );
        }

        // GET: api/RegistroEntradas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroEntrada>> GetRegistroEntrada(int id)
        {
            return await HandleDbOperation<RegistroEntrada>(
                async () => await _context.RegistroEntradas
                    .Include(r => r.Usuario)
                    .Include(r => r.Proveedor)
                    .FirstOrDefaultAsync(r => r.RegistroEntradaID == id),
                $"Error retrieving registro entrada with ID {id}"
            );
        }

        // PUT: api/RegistroEntradas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroEntrada(int id, RegistroEntrada registroEntrada)
        {
            if (id != registroEntrada.RegistroEntradaID)
            {
                return BadRequest();
            }

            _context.Entry(registroEntrada).State = EntityState.Modified;

            return await HandleDbUpdate<RegistroEntrada>(
                registroEntrada,
                async () => await _context.SaveChangesAsync(),
                $"Error updating registro entrada with ID {id}"
            );
        }

        // POST: api/RegistroEntradas
        [HttpPost]
        public async Task<ActionResult<RegistroEntrada>> PostRegistroEntrada(RegistroEntrada registroEntrada)
        {
            return await HandleDbCreate<RegistroEntrada>(
                registroEntrada,
                async () =>
                {
                    _context.RegistroEntradas.Add(registroEntrada);
                    await _context.SaveChangesAsync();
                },
                "Error creating registro entrada"
            );
        }

        // DELETE: api/RegistroEntradas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistroEntrada(int id)
        {
            return await HandleDbDelete<RegistroEntrada>(
                async () => await _context.RegistroEntradas.FindAsync(id),
                async (registroEntrada) =>
                {
                    _context.RegistroEntradas.Remove(registroEntrada);
                    await _context.SaveChangesAsync();
                },
                $"Error deleting registro entrada with ID {id}"
            );
        }
    }
}
