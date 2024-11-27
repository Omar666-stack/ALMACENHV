using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;

namespace ALMACENHV.Controllers
{
    public class ProductosController : BaseController
    {
        private readonly TuDbContext _context;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(TuDbContext context, ILogger<ProductosController> logger)
            : base(logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<>>> GetProductos()
        {
            return await HandleDbOperation(async () =>
            {
                var items = await _context.Productos.ToListAsync();
                if (!items.Any())
                {
                    _logger.LogInformation("No se encontraron registros");
                    return new List<>();
                }
                return items;
            });
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<>> Get(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.Productos.FindAsync(id);
                if (item == null)
                {
                    _logger.LogWarning("Registro no encontrado: {Id}", id);
                    return null;
                }
                return item;
            });
        }

        // PUT: api/Productos/5
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

        // POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<>> Post( item)
        {
            return await HandleDbOperation(async () =>
            {
                _context.Productos.Add(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.Productos.FindAsync(id);
                if (item == null)
                {
                    return null;
                }

                _context.Productos.Remove(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }
    }
}
