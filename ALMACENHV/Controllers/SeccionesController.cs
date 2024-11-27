using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;

namespace ALMACENHV.Controllers
{
    public class SeccionesController : BaseController
    {
        private readonly TuDbContext _context;
        private readonly ILogger<SeccionesController> _logger;

        public SeccionesController(TuDbContext context, ILogger<SeccionesController> logger)
            : base(logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Secciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<>>> GetSecciones()
        {
            return await HandleDbOperation(async () =>
            {
                var items = await _context.Secciones.ToListAsync();
                if (!items.Any())
                {
                    _logger.LogInformation("No se encontraron registros");
                    return new List<>();
                }
                return items;
            });
        }

        // GET: api/Secciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<>> Get(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.Secciones.FindAsync(id);
                if (item == null)
                {
                    _logger.LogWarning("Registro no encontrado: {Id}", id);
                    return null;
                }
                return item;
            });
        }

        // PUT: api/Secciones/5
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

        // POST: api/Secciones
        [HttpPost]
        public async Task<ActionResult<>> Post( item)
        {
            return await HandleDbOperation(async () =>
            {
                _context.Secciones.Add(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }

        // DELETE: api/Secciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.Secciones.FindAsync(id);
                if (item == null)
                {
                    return null;
                }

                _context.Secciones.Remove(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }
    }
}
