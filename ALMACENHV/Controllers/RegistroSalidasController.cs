using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;

namespace ALMACENHV.Controllers
{
    public class RegistroSalidasController : BaseController
    {
        private readonly TuDbContext _context;
        private readonly ILogger<RegistroSalidasController> _logger;

        public RegistroSalidasController(TuDbContext context, ILogger<RegistroSalidasController> logger)
            : base(logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/RegistroSalidas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<>>> GetRegistroSalidas()
        {
            return await HandleDbOperation(async () =>
            {
                var items = await _context.RegistroSalidas.ToListAsync();
                if (!items.Any())
                {
                    _logger.LogInformation("No se encontraron registros");
                    return new List<>();
                }
                return items;
            });
        }

        // GET: api/RegistroSalidas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<>> Get(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.RegistroSalidas.FindAsync(id);
                if (item == null)
                {
                    _logger.LogWarning("Registro no encontrado: {Id}", id);
                    return null;
                }
                return item;
            });
        }

        // PUT: api/RegistroSalidas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,  item)
        {
            if (id != item.ID)
            {
                return BadRequest("El ID no coincide con el registro a actualizar");
            }

            return await HandleDbOperation(async () =>
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return item;
            });
        }

        // POST: api/RegistroSalidas
        [HttpPost]
        public async Task<ActionResult<>> Post( item)
        {
            return await HandleDbOperation(async () =>
            {
                _context.RegistroSalidas.Add(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }

        // DELETE: api/RegistroSalidas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.RegistroSalidas.FindAsync(id);
                if (item == null)
                {
                    return null;
                }

                _context.RegistroSalidas.Remove(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }
    }
}
