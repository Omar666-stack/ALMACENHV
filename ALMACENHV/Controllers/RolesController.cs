using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;
using Microsoft.AspNetCore.Authorization;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : BaseController
    {
        public RolesController(AlmacenContext context, ILogger<RolesController> logger)
            : base(context, logger)
        {
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rol>>> GetRoles()
        {
            return await HandleDbOperationList(async () =>
                await _context.Roles
                    .Where(r => r.Activo)
                    .ToListAsync());
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            return await HandleDbOperation(async () =>
                await _context.Roles
                    .FirstOrDefaultAsync(r => r.RolID == id));
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> PutRol(int id, Rol rol)
        {
            if (id != rol.RolID)
            {
                return BadRequest();
            }

            return await HandleDbUpdate(rol, async () =>
            {
                rol.FechaModificacion = DateTime.UtcNow;
                _context.Entry(rol).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            });
        }

        // POST: api/Roles
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<Rol>> PostRol(Rol rol)
        {
            return await HandleDbCreate(rol, async () =>
            {
                rol.FechaCreacion = DateTime.UtcNow;
                _context.Roles.Add(rol);
                await _context.SaveChangesAsync();
            });
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            return await HandleDbDelete(rol, async () =>
            {
                rol.Activo = false;
                rol.FechaModificacion = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            });
        }

        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.RolID == id);
        }
    }
}
