using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace ALMACENHV.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : BaseController
    {
        private readonly IMemoryCache _cache;
        private const string ProductosKey = "productos_all";
        private const string ProductoKey = "producto_";

        public ProductosController(AlmacenContext context, ILogger<ProductosController> logger, IMemoryCache cache) 
            : base(context, logger)
        {
            _cache = cache;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await HandleDbOperationList(async () =>
            {
                if (!_cache.TryGetValue(ProductosKey, out List<Producto> productos))
                {
                    productos = await _context.Productos
                        .Include(p => p.Ubicacion)
                        .Where(p => p.Activo)
                        .ToListAsync();

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                    _cache.Set(ProductosKey, productos, cacheEntryOptions);
                }
                return productos;
            });
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var cacheKey = $"{ProductoKey}{id}";
                if (!_cache.TryGetValue(cacheKey, out Producto producto))
                {
                    producto = await _context.Productos
                        .Include(p => p.Ubicacion)
                        .FirstOrDefaultAsync(p => p.ProductoID == id && p.Activo);

                    if (producto != null)
                    {
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                        _cache.Set(cacheKey, producto, cacheEntryOptions);
                    }
                }
                return producto;
            });
        }

        // PUT: api/Productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.ProductoID)
            {
                return BadRequest();
            }

            return await HandleDbUpdate(producto, async () =>
            {
                // Validar que la ubicación existe y está activa
                var ubicacion = await _context.Ubicaciones
                    .FirstOrDefaultAsync(u => u.UbicacionID == producto.UbicacionID && u.Activo);
                
                if (ubicacion == null)
                {
                    throw new InvalidOperationException("La ubicación especificada no existe o no está activa");
                }

                if (producto.Stock < producto.StockMinimo)
                {
                    throw new InvalidOperationException("El stock no puede ser menor al stock mínimo");
                }

                producto.FechaModificacion = DateTime.UtcNow;
                _context.Entry(producto).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _cache.Remove(ProductosKey);
                _cache.Remove($"{ProductoKey}{id}");
            });
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            return await HandleDbCreate(producto, async () =>
            {
                // Validar que la ubicación existe y está activa
                var ubicacion = await _context.Ubicaciones
                    .FirstOrDefaultAsync(u => u.UbicacionID == producto.UbicacionID && u.Activo);
                
                if (ubicacion == null)
                {
                    throw new InvalidOperationException("La ubicación especificada no existe o no está activa");
                }

                if (producto.Stock < producto.StockMinimo)
                {
                    throw new InvalidOperationException("El stock inicial no puede ser menor al stock mínimo");
                }

                producto.FechaCreacion = DateTime.UtcNow;
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                _cache.Remove(ProductosKey);
            });
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            return await HandleDbDelete<Producto>(null, async () =>
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null)
                {
                    return;
                }

                producto.Activo = false;
                await _context.SaveChangesAsync();
                _cache.Remove(ProductosKey);
                _cache.Remove($"{ProductoKey}{id}");
            });
        }

        // GET: api/Productos/Stock/{id}
        [HttpGet("Stock/{id}")]
        public async Task<ActionResult<int>> GetStock(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            return producto.Stock;
        }

        // PUT: api/Productos/Stock/{id}
        [HttpPut("Stock/{id}")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int cantidad)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null || !producto.Activo)
            {
                return NotFound();
            }

            int nuevoStock = producto.Stock + cantidad;
            
            if (nuevoStock < 0)
            {
                return BadRequest("No hay suficiente stock disponible");
            }

            if (nuevoStock < producto.StockMinimo)
            {
                // Podríamos agregar aquí lógica para notificar stock bajo
                _logger.LogWarning($"Stock bajo para producto {producto.Codigo}: {nuevoStock} unidades (mínimo: {producto.StockMinimo})");
            }

            producto.Stock = nuevoStock;
            producto.FechaModificacion = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            _cache.Remove(ProductosKey);
            _cache.Remove($"{ProductoKey}{id}");

            return NoContent();
        }
    }
}
