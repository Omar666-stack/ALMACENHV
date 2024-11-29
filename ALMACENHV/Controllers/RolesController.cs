using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            return await HandleDbOperationList<Rol>(
                async () => await _context.Roles
                    .Include(r => r.Usuarios)
                    .ToListAsync(),
                "Error retrieving roles"
            );
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            return await HandleDbOperation<Rol>(
                async () => await _context.Roles
                    .Include(r => r.Usuarios)
                    .FirstOrDefaultAsync(r => r.RolID == id),
                $"Error retrieving rol with ID {id}"
            );
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, Rol rol)
        {
            if (id != rol.RolID)
            {
                return BadRequest();
            }

            _context.Entry(rol).State = EntityState.Modified;

            return await HandleDbUpdate<Rol>(
                rol,
                async () => await _context.SaveChangesAsync(),
                $"Error updating rol with ID {id}"
            );
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<Rol>> PostRol(Rol rol)
        {
            return await HandleDbCreate<Rol>(
                rol,
                async () =>
                {
                    _context.Roles.Add(rol);
                    await _context.SaveChangesAsync();
                },
                "Error creating rol"
            );
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            return await HandleDbDelete<Rol>(
                async () => await _context.Roles.FindAsync(id),
                async (rol) =>
                {
                    _context.Roles.Remove(rol);
                    await _context.SaveChangesAsync();
                },
                $"Error deleting rol with ID {id}"
            );
        }
    }
}
