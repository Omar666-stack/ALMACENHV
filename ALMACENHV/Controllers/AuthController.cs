using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ALMACENHV.Models;
using ALMACENHV.Data;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IConfiguration _configuration;

        public AuthController(AlmacenContext context, ILogger<AuthController> logger, IConfiguration configuration)
            : base(context, logger)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Usuario>> Register(RegistroRequest request)
        {
            try
            {
                if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
                {
                    return BadRequest("El correo electrónico ya está registrado");
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                var usuario = new Usuario
                {
                    NombreUsuario = request.NombreUsuario,
                    Email = request.Email,
                    Password = passwordHash,
                    RolID = request.RolID,
                    CargoID = request.CargoID
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario");
                return StatusCode(500, "Error interno del servidor al registrar usuario");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginRequest request)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (usuario == null)
                {
                    return Unauthorized("Usuario o contraseña incorrectos");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.Password))
                {
                    return Unauthorized("Usuario o contraseña incorrectos");
                }

                string token = CreateToken(usuario);

                var response = new
                {
                    token = token,
                    nombreUsuario = usuario.NombreUsuario,
                    usuarioID = usuario.UsuarioID,
                    rolID = usuario.RolID
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el proceso de login");
                return StatusCode(500, "Error interno del servidor durante el login");
            }
        }

        private string CreateToken(Usuario usuario)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Usuario"),
                new Claim("UsuarioID", usuario.UsuarioID.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds,
                issuer: "https://almacenhv.onrender.com",
                audience: "https://almacenhv.onrender.com"
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var usuario = await _context.Usuarios.FindAsync(model.UsuarioID);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, usuario.Password))
            {
                return BadRequest("La contraseña actual es incorrecta");
            }

            usuario.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Contraseña cambiada para el usuario: {usuario.Email}");

            return Ok("Contraseña actualizada correctamente");
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<Usuario>> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("Token inválido");
                }

                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .Include(u => u.Cargo)
                    .FirstOrDefaultAsync(u => u.UsuarioID == int.Parse(userId));

                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario actual");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegistroRequest
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RolID { get; set; }
        public int CargoID { get; set; }
    }

    public class ChangePasswordModel
    {
        public int UsuarioID { get; set; }
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
