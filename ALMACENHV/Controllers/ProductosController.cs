using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : BaseController
    {
        private readonly IMemoryCache _cache;
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> _locks = new();
        private const string CACHE_KEY_ALL_PRODUCTOS = "AllProductos";
        private const string CACHE_KEY_PRODUCTO = "Producto_";
        private readonly TimeSpan CACHE_DURATION = TimeSpan.FromMinutes(15);

        public ProductosController(AlmacenContext context, ILogger<ProductosController> logger, IMemoryCache cache)
            : base(context, logger)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            try
            {
                // Intentar obtener productos del caché
                if (_cache.TryGetValue(CACHE_KEY_ALL_PRODUCTOS, out IEnumerable<Producto> cachedProductos))
                {
                    return Ok(cachedProductos);
                }

                // Si no está en caché, obtener de la base de datos
                var productos = await _context.Productos
                    .AsNoTracking()
                    .Include(p => p.Seccion)
                    .AsSplitQuery()
                    .ToListAsync();

                // Guardar en caché
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(CACHE_DURATION)
                    .SetPriority(CacheItemPriority.Normal);

                _cache.Set(CACHE_KEY_ALL_PRODUCTOS, productos, cacheEntryOptions);

                return Ok(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            try
            {
                string cacheKey = $"{CACHE_KEY_PRODUCTO}{id}";

                // Intentar obtener producto del caché
                if (_cache.TryGetValue(cacheKey, out Producto cachedProducto))
                {
                    return Ok(cachedProducto);
                }

                // Obtener o crear el semáforo para este ID
                var lockObj = _locks.GetOrAdd(id, _ => new SemaphoreSlim(1, 1));
                await lockObj.WaitAsync();

                try
                {
                    // Verificar caché nuevamente después de obtener el lock
                    if (_cache.TryGetValue(cacheKey, out cachedProducto))
                    {
                        return Ok(cachedProducto);
                    }

                    var producto = await _context.Productos
                        .AsNoTracking()
                        .Include(p => p.Seccion)
                        .AsSplitQuery()
                        .FirstOrDefaultAsync(p => p.ProductoID == id);

                    if (producto == null)
                    {
                        return NotFound($"Producto con ID {id} no encontrado");
                    }

                    // Guardar en caché
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(CACHE_DURATION)
                        .SetPriority(CacheItemPriority.Normal);

                    _cache.Set(cacheKey, producto, cacheEntryOptions);

                    return Ok(producto);
                }
                finally
                {
                    lockObj.Release();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener producto con ID {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/Productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.ProductoID)
            {
                return BadRequest();
            }

            try
            {
                // Validar que el código no esté duplicado
                var codigoExiste = await _context.Productos
                    .AsNoTracking()
                    .AnyAsync(p => p.Codigo == producto.Codigo && p.ProductoID != id);

                if (codigoExiste)
                {
                    return BadRequest("Ya existe otro producto con este código");
                }

                _context.Entry(producto).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Invalidar caché
                _cache.Remove(CACHE_KEY_ALL_PRODUCTOS);
                _cache.Remove($"{CACHE_KEY_PRODUCTO}{id}");

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductoExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar producto con ID {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            try
            {
                // Validar que el código no esté duplicado
                var codigoExiste = await _context.Productos
                    .AsNoTracking()
                    .AnyAsync(p => p.Codigo == producto.Codigo);

                if (codigoExiste)
                {
                    return BadRequest("Ya existe un producto con este código");
                }

                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();

                // Invalidar caché de todos los productos
                _cache.Remove(CACHE_KEY_ALL_PRODUCTOS);

                return CreatedAtAction(nameof(GetProducto), new { id = producto.ProductoID }, producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }

                // Verificar si el producto tiene registros asociados
                var tieneRegistros = await _context.RegistroEntradas.AnyAsync(r => r.ProductoID == id) ||
                                   await _context.RegistroSalidas.AnyAsync(r => r.ProductoID == id);

                if (tieneRegistros)
                {
                    return BadRequest("No se puede eliminar el producto porque tiene registros asociados");
                }

                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();

                // Invalidar caché
                _cache.Remove(CACHE_KEY_ALL_PRODUCTOS);
                _cache.Remove($"{CACHE_KEY_PRODUCTO}{id}");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar producto con ID {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        private async Task<bool> ProductoExists(int id)
        {
            return await _context.Productos.AnyAsync(e => e.ProductoID == id);
        }
    }
}
