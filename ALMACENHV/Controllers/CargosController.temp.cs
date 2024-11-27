using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;

namespace ALMACENHV.Controllers
{
    public class CargosController : BaseController
    {
        private readonly TuDbContext _context;
        private readonly ILogger<CargosController> _logger;

        public CargosController(TuDbContext context, ILogger<CargosController> logger)
            : base(logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Cargos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cargo>>> GetCargos()
        {
            return await HandleDbOperation(async () =>
            {
                var cargos = await _context.Cargos.ToListAsync();
                if (!cargos.Any())
                {
                    _logger.LogInformation("No se encontraron cargos");
                    return new List<Cargo>();
                }
                return cargos;
            });
        }

        // GET: api/Cargos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cargo>> GetCargo(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var cargo = await _context.Cargos.FindAsync(id);
                if (cargo == null)
                {
                    _logger.LogWarning("Cargo no encontrado: {Id}", id);
                    return null;
                }
                return cargo;
            });
        }

        // PUT: api/Cargos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCargo(int id, Cargo cargo)
        {
            if (id != cargo.CargoID)
            {
                return BadRequest("El ID no coincide con el cargo a actualizar");
            }

            return await HandleDbOperation(async () =>
            {
                _context.Entry(cargo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return cargo;
            });
        }

        // POST: api/Cargos
        [HttpPost]
        public async Task<ActionResult<Cargo>> PostCargo(Cargo cargo)
        {
            return await HandleDbOperation(async () =>
            {
                _context.Cargos.Add(cargo);
                await _context.SaveChangesAsync();
                return cargo;
            });
        }

        // DELETE: api/Cargos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCargo(int id)
        {
            return await HandleDbOperation(async () =>
            {
                var cargo = await _context.Cargos.FindAsync(id);
                if (cargo == null)
                {
                    return null;
                }

                _context.Cargos.Remove(cargo);
                await _context.SaveChangesAsync();
                return cargo;
            });
        }
    }
}
