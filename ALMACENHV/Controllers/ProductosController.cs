using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : BaseController
    {
        public ProductosController(AlmacenContext context, ILogger<ProductosController> logger)
            : base(context, logger)
        {
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await HandleDbOperationList<Producto>(
                async () => await _context.Productos
                    .Include(p => p.Seccion)
                    .ToListAsync(),
                "Error retrieving productos"
            );
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            return await HandleDbOperation<Producto>(
                async () => await _context.Productos
                    .Include(p => p.Seccion)
                    .FirstOrDefaultAsync(p => p.ProductoID == id),
                $"Error retrieving producto with ID {id}"
            );
        }

        // PUT: api/Productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.ProductoID)
            {
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;

            return await HandleDbUpdate<Producto>(
                producto,
                async () => await _context.SaveChangesAsync(),
                $"Error updating producto with ID {id}"
            );
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            return await HandleDbCreate<Producto>(
                producto,
                async () =>
                {
                    _context.Productos.Add(producto);
                    await _context.SaveChangesAsync();
                },
                "Error creating producto"
            );
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            return await HandleDbDelete<Producto>(
                async () => await _context.Productos.FindAsync(id),
                async (producto) =>
                {
                    _context.Productos.Remove(producto);
                    await _context.SaveChangesAsync();
                },
                $"Error deleting producto with ID {id}"
            );
        }
    }
}
