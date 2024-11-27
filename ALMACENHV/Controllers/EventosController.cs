using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;

namespace ALMACENHV.Controllers
{
    public class EventosController : BaseController
    {
        private readonly TuDbContext _context;
        private readonly ILogger<EventosController> _logger;

        public EventosController(TuDbContext context, ILogger<EventosController> logger)
            : base(logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Eventos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<>>> GetEventos()
        {
            return await HandleDbOperation(async () =>
            {
                var items = await _context.Eventos.ToListAsync();
                if (!items.Any())
                {
                    _logger.LogInformation("No se encontraron registros");
                    return new List<>();
                }
                return items;
            });
        }

        // GET: api/Eventos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<>> Get(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.Eventos.FindAsync(id);
                if (item == null)
                {
                    _logger.LogWarning("Registro no encontrado: {Id}", id);
                    return null;
                }
                return item;
            });
        }

        // PUT: api/Eventos/5
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

        // POST: api/Eventos
        [HttpPost]
        public async Task<ActionResult<>> Post( item)
        {
            return await HandleDbOperation(async () =>
            {
                _context.Eventos.Add(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }

        // DELETE: api/Eventos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.Eventos.FindAsync(id);
                if (item == null)
                {
                    return null;
                }

                _context.Eventos.Remove(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }
    }
}
