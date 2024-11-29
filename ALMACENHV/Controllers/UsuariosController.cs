using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : BaseController
    {
        private new readonly AlmacenContext _context;
        private new readonly ILogger<UsuariosController> _logger;

        public UsuariosController(AlmacenContext context, ILogger<UsuariosController> logger)
            : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await HandleDbOperationList<Usuario>(
                async () => await _context.Usuarios
                    .Include(u => u.Cargo)
                    .Include(u => u.Rol)
                    .Include(u => u.RegistroIngresos)
                    .ToListAsync(),
                "Error retrieving usuarios"
            );
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            return await HandleDbOperation<Usuario>(
                async () => await _context.Usuarios
                    .Include(u => u.Cargo)
                    .Include(u => u.Rol)
                    .Include(u => u.RegistroIngresos)
                    .FirstOrDefaultAsync(u => u.UsuarioID == id),
                $"Error retrieving usuario with ID {id}"
            );
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.UsuarioID)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            return await HandleDbUpdate<Usuario>(
                usuario,
                async () => await _context.SaveChangesAsync(),
                $"Error updating usuario with ID {id}"
            );
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            return await HandleDbCreate<Usuario>(
                usuario,
                async () =>
                {
                    _context.Usuarios.Add(usuario);
                    await _context.SaveChangesAsync();
                },
                "Error creating usuario"
            );
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            return await HandleDbDelete<Usuario>(
                async () => await _context.Usuarios.FindAsync(id),
                async (usuario) =>
                {
                    _context.Usuarios.Remove(usuario);
                    await _context.SaveChangesAsync();
                },
                $"Error deleting usuario with ID {id}"
            );
        }
    }
}
