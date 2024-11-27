using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;

namespace ALMACENHV.Controllers
{
    public class RolesController : BaseController
    {
        private readonly TuDbContext _context;
        private readonly ILogger<RolesController> _logger;

        public RolesController(TuDbContext context, ILogger<RolesController> logger)
            : base(logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<>>> GetRoles()
        {
            return await HandleDbOperation(async () =>
            {
                var items = await _context.Roles.ToListAsync();
                if (!items.Any())
                {
                    _logger.LogInformation("No se encontraron registros");
                    return new List<>();
                }
                return items;
            });
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<>> Get(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.Roles.FindAsync(id);
                if (item == null)
                {
                    _logger.LogWarning("Registro no encontrado: {Id}", id);
                    return null;
                }
                return item;
            });
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,  item)
        {
            if (id != item.ID)
            {
                return BadRequest("El ID no coincide con el registro a actualizar");
            }

            return await HandleDbOperation(async () =>
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return item;
            });
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<>> Post( item)
        {
            return await HandleDbOperation(async () =>
            {
                _context.Roles.Add(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var item = await _context.Roles.FindAsync(id);
                if (item == null)
                {
                    return null;
                }

                _context.Roles.Remove(item);
                await _context.SaveChangesAsync();
                return item;
            });
        }
    }
}
