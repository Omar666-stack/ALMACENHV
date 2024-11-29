using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : BaseController
    {
        private readonly IConfiguration _configuration;

        public UsuariosController(AlmacenContext context, ILogger<UsuariosController> logger, IConfiguration configuration)
            : base(context, logger)
        {
            _configuration = configuration;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await HandleDbOperationList(async () =>
                await _context.Usuarios
                    .Include(u => u.Rol)
                    .Where(u => u.Activo)
                    .ToListAsync());
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            return await HandleDbOperation(async () =>
                await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.UsuarioID == id));
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.UsuarioID)
            {
                return BadRequest();
            }

            return await HandleDbUpdate(usuario, async () =>
            {
                usuario.FechaModificacion = DateTime.UtcNow;
                _context.Entry(usuario).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            });
        }

        // POST: api/Usuarios
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            return await HandleDbCreate(usuario, async () =>
            {
                usuario.FechaCreacion = DateTime.UtcNow;
                usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
            });
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return await HandleDbDelete(usuario, async () =>
            {
                usuario.Activo = false;
                usuario.FechaModificacion = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            });
        }

        // POST: api/Usuarios/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Login([FromBody] LoginModel model)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.NombreUsuario == model.NombreUsuario && u.Activo);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuario.Password))
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }

            var token = GenerateJwtToken(usuario);
            return Ok(new { token });
        }

        private string GenerateJwtToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Token"] ?? throw new InvalidOperationException("JWT Token not found"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UsuarioID.ToString()),
                    new Claim(ClaimTypes.Name, user.NombreUsuario),
                    new Claim(ClaimTypes.Role, user.Rol?.Nombre ?? "Usuario"),
                    new Claim("FullName", $"{user.Nombre} {user.Apellido}")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioID == id);
        }
    }

    public class LoginModel
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
