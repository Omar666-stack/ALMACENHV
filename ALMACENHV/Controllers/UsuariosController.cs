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
    public class UsuariosController : BaseController
    {
        private readonly IMemoryCache _cache;
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> _locks = new();
        private const string CACHE_KEY_ALL_USERS = "AllUsers";
        private const string CACHE_KEY_USER = "User_";
        private readonly TimeSpan CACHE_DURATION = TimeSpan.FromMinutes(15);
        private new readonly AlmacenContext _context;
        private new readonly ILogger<UsuariosController> _logger;

        public UsuariosController(AlmacenContext context, ILogger<UsuariosController> logger, IMemoryCache cache)
            : base(context, logger)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            try
            {
                // Intentar obtener usuarios del caché
                if (_cache.TryGetValue(CACHE_KEY_ALL_USERS, out IEnumerable<Usuario> cachedUsers))
                {
                    return Ok(cachedUsers);
                }

                // Si no está en caché, obtener de la base de datos
                var usuarios = await _context.Usuarios
                    .AsNoTracking()
                    .Include(u => u.Cargo)
                    .Include(u => u.Rol)
                    .AsSplitQuery()
                    .ToListAsync();

                // Guardar en caché
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(CACHE_DURATION)
                    .SetPriority(CacheItemPriority.Normal);

                _cache.Set(CACHE_KEY_ALL_USERS, usuarios, cacheEntryOptions);

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            try
            {
                string cacheKey = $"{CACHE_KEY_USER}{id}";

                // Intentar obtener usuario del caché
                if (_cache.TryGetValue(cacheKey, out Usuario cachedUser))
                {
                    return Ok(cachedUser);
                }

                // Obtener o crear el semáforo para este ID
                var lockObj = _locks.GetOrAdd(id, _ => new SemaphoreSlim(1, 1));
                await lockObj.WaitAsync();

                try
                {
                    // Verificar caché nuevamente después de obtener el lock
                    if (_cache.TryGetValue(cacheKey, out cachedUser))
                    {
                        return Ok(cachedUser);
                    }

                    var usuario = await _context.Usuarios
                        .AsNoTracking()
                        .Include(u => u.Cargo)
                        .Include(u => u.Rol)
                        .AsSplitQuery()
                        .FirstOrDefaultAsync(u => u.UsuarioID == id);

                    if (usuario == null)
                    {
                        return NotFound($"Usuario con ID {id} no encontrado");
                    }

                    // Guardar en caché
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(CACHE_DURATION)
                        .SetPriority(CacheItemPriority.Normal);

                    _cache.Set(cacheKey, usuario, cacheEntryOptions);

                    return Ok(usuario);
                }
                finally
                {
                    lockObj.Release();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener usuario con ID {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.UsuarioID)
            {
                return BadRequest();
            }

            try
            {
                _context.Entry(usuario).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Invalidar caché
                _cache.Remove(CACHE_KEY_ALL_USERS);
                _cache.Remove($"{CACHE_KEY_USER}{id}");

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UsuarioExists(id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            try
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Invalidar caché de todos los usuarios
                _cache.Remove(CACHE_KEY_ALL_USERS);

                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.UsuarioID }, usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                // Invalidar caché
                _cache.Remove(CACHE_KEY_ALL_USERS);
                _cache.Remove($"{CACHE_KEY_USER}{id}");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar usuario con ID {id}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        private async Task<bool> UsuarioExists(int id)
        {
            return await _context.Usuarios.AnyAsync(e => e.UsuarioID == id);
        }
    }
}
