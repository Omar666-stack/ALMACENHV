using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargosController : BaseController
    {
        private new readonly AlmacenContext _context;
        private new readonly ILogger<CargosController> _logger;

        public CargosController(AlmacenContext context, ILogger<CargosController> logger)
            : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Cargos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cargo>>> GetCargos()
        {
            return await HandleDbOperationList<Cargo>(
                async () => await _context.Cargos
                    .Include(c => c.Usuarios)
                    .ToListAsync(),
                "Error retrieving cargos"
            );
        }

        // GET: api/Cargos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cargo>> GetCargo(int id)
        {
            return await HandleDbOperation<Cargo>(
                async () => await _context.Cargos
                    .Include(c => c.Usuarios)
                    .FirstOrDefaultAsync(c => c.CargoID == id),
                $"Error retrieving cargo with ID {id}"
            );
        }

        // PUT: api/Cargos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCargo(int id, Cargo cargo)
        {
            if (id != cargo.CargoID)
            {
                return BadRequest();
            }

            _context.Entry(cargo).State = EntityState.Modified;

            return await HandleDbUpdate<Cargo>(
                cargo,
                async () => await _context.SaveChangesAsync(),
                $"Error updating cargo with ID {id}"
            );
        }

        // POST: api/Cargos
        [HttpPost]
        public async Task<ActionResult<Cargo>> PostCargo(Cargo cargo)
        {
            return await HandleDbCreate<Cargo>(
                cargo,
                async () =>
                {
                    _context.Cargos.Add(cargo);
                    await _context.SaveChangesAsync();
                },
                "Error creating cargo"
            );
        }

        // DELETE: api/Cargos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCargo(int id)
        {
            return await HandleDbDelete<Cargo>(
                async () => await _context.Cargos.FindAsync(id),
                async (cargo) =>
                {
                    _context.Cargos.Remove(cargo);
                    await _context.SaveChangesAsync();
                },
                $"Error deleting cargo with ID {id}"
            );
        }
    }
}
