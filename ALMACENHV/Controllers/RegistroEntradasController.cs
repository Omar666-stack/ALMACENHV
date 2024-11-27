using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;

namespace ALMACENHV.Controllers
{
    public class RegistroEntradasController : BaseController
    {
        private readonly TuDbContext _context;
        private readonly ILogger<RegistroEntradasController> _logger;

        public RegistroEntradasController(TuDbContext context, ILogger<RegistroEntradasController> logger)
            : base(logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/RegistroEntradas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<>>> GetRegistroEntradas()
        {
            return await HandleDbOperation(async () =>
            {
                var items = await _context.RegistroEntradas.ToListAsync();
                if (!items.Any())
                {
                    _logger.LogInformation("No se encontraron registros");
                    return new List<>();
                }
                return items;
            });
        }

        // GET: api/RegistroEntradas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<>> Get(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.RegistroEntradas.FindAsync(id);
                if (item == null)
                {
                    _logger.LogWarning("Registro no encontrado: {Id}", id);
                    return null;
                }
                return item;
            });
        }

        // PUT: api/RegistroEntradas/5
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

        // POST: api/RegistroEntradas
        [HttpPost]
        public async Task<ActionResult<>> Post( item)
        {
            return await HandleDbOperation(async () =>
            {
                _context.RegistroEntradas.Add(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }

        // DELETE: api/RegistroEntradas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.RegistroEntradas.FindAsync(id);
                if (item == null)
                {
                    return null;
                }

                _context.RegistroEntradas.Remove(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }
    }
}
