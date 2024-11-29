using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroSalidasController : BaseController
    {
        private new readonly AlmacenContext _context;
        private new readonly ILogger<RegistroSalidasController> _logger;

        public RegistroSalidasController(AlmacenContext context, ILogger<RegistroSalidasController> logger)
            : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/RegistroSalidas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroSalida>>> GetRegistroSalidas()
        {
            return await HandleDbOperationList<RegistroSalida>(
                async () => await _context.RegistroSalidas
                    .Include(r => r.Usuario)
                    .Include(r => r.Ubicacion)
                    .ToListAsync(),
                "Error retrieving registro salidas"
            );
        }

        // GET: api/RegistroSalidas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroSalida>> GetRegistroSalida(int id)
        {
            return await HandleDbOperation<RegistroSalida>(
                async () => await _context.RegistroSalidas
                    .Include(r => r.Usuario)
                    .Include(r => r.Ubicacion)
                    .FirstOrDefaultAsync(r => r.RegistroSalidaID == id),
                $"Error retrieving registro salida with ID {id}"
            );
        }

        // PUT: api/RegistroSalidas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroSalida(int id, RegistroSalida registroSalida)
        {
            if (id != registroSalida.RegistroSalidaID)
            {
                return BadRequest();
            }

            _context.Entry(registroSalida).State = EntityState.Modified;

            return await HandleDbUpdate<RegistroSalida>(
                registroSalida,
                async () => await _context.SaveChangesAsync(),
                $"Error updating registro salida with ID {id}"
            );
        }

        // POST: api/RegistroSalidas
        [HttpPost]
        public async Task<ActionResult<RegistroSalida>> PostRegistroSalida(RegistroSalida registroSalida)
        {
            return await HandleDbCreate<RegistroSalida>(
                registroSalida,
                async () =>
                {
                    _context.RegistroSalidas.Add(registroSalida);
                    await _context.SaveChangesAsync();
                },
                "Error creating registro salida"
            );
        }

        // DELETE: api/RegistroSalidas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistroSalida(int id)
        {
            return await HandleDbDelete<RegistroSalida>(
                async () => await _context.RegistroSalidas.FindAsync(id),
                async (registroSalida) =>
                {
                    _context.RegistroSalidas.Remove(registroSalida);
                    await _context.SaveChangesAsync();
                },
                $"Error deleting registro salida with ID {id}"
            );
        }
    }
}
