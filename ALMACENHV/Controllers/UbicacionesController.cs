using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;

namespace ALMACENHV.Controllers
{
    public class UbicacionesController : BaseController
    {
        private readonly TuDbContext _context;
        private readonly ILogger<UbicacionesController> _logger;

        public UbicacionesController(TuDbContext context, ILogger<UbicacionesController> logger)
            : base(logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Ubicaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<>>> GetUbicaciones()
        {
            return await HandleDbOperation(async () =>
            {
                var items = await _context.Ubicaciones.ToListAsync();
                if (!items.Any())
                {
                    _logger.LogInformation("No se encontraron registros");
                    return new List<>();
                }
                return items;
            });
        }

        // GET: api/Ubicaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<>> Get(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.Ubicaciones.FindAsync(id);
                if (item == null)
                {
                    _logger.LogWarning("Registro no encontrado: {Id}", id);
                    return null;
                }
                return item;
            });
        }

        // PUT: api/Ubicaciones/5
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

        // POST: api/Ubicaciones
        [HttpPost]
        public async Task<ActionResult<>> Post( item)
        {
            return await HandleDbOperation(async () =>
            {
                _context.Ubicaciones.Add(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }

        // DELETE: api/Ubicaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.Ubicaciones.FindAsync(id);
                if (item == null)
                {
                    return null;
                }

                _context.Ubicaciones.Remove(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }
    }
}
