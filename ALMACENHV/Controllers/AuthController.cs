using Microsoft.AspNetCore.Mvc;
using ALMACENHV.Models;
using ALMACENHV.Services;
using Microsoft.EntityFrameworkCore;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TuDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(TuDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        /// <summary>
        /// Inicia sesión de usuario
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }

            if (!_authService.VerifyPassword(request.Password, usuario.Password))
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }

            var token = _authService.GenerateJwtToken(usuario);

            return Ok(new LoginResponse
            {
                Token = token,
                NombreUsuario = usuario.NombreUsuario,
                UsuarioID = usuario.UsuarioID,
                RolID = usuario.RolID
            });
        }

        /// <summary>
        /// Registra un nuevo usuario
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("El email ya está registrado");
            }

            var usuario = new Usuario
            {
                NombreUsuario = request.NombreUsuario,
                Email = request.Email,
                Password = _authService.HashPassword(request.Password),
                CargoID = request.CargoID,
                RolID = request.RolID
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var token = _authService.GenerateJwtToken(usuario);

            return Ok(new LoginResponse
            {
                Token = token,
                NombreUsuario = usuario.NombreUsuario,
                UsuarioID = usuario.UsuarioID,
                RolID = usuario.RolID
            });
        }
    }
}
