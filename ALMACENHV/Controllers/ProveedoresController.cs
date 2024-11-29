using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : BaseController
    {
        private new readonly AlmacenContext _context;
        private new readonly ILogger<ProveedoresController> _logger;

        public ProveedoresController(AlmacenContext context, ILogger<ProveedoresController> logger)
            : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Proveedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> GetProveedores()
        {
            return await HandleDbOperationList<Proveedor>(
                async () => await _context.Proveedores
                    .Include(p => p.RegistroIngresos)
                    .ToListAsync(),
                "Error retrieving proveedores"
            );
        }

        // GET: api/Proveedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> GetProveedor(int id)
        {
            return await HandleDbOperation<Proveedor>(
                async () => await _context.Proveedores
                    .Include(p => p.RegistroIngresos)
                    .FirstOrDefaultAsync(p => p.ProveedorID == id),
                $"Error retrieving proveedor with ID {id}"
            );
        }

        // PUT: api/Proveedores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProveedor(int id, Proveedor proveedor)
        {
            if (id != proveedor.ProveedorID)
            {
                return BadRequest();
            }

            _context.Entry(proveedor).State = EntityState.Modified;

            return await HandleDbUpdate<Proveedor>(
                proveedor,
                async () => await _context.SaveChangesAsync(),
                $"Error updating proveedor with ID {id}"
            );
        }

        // POST: api/Proveedores
        [HttpPost]
        public async Task<ActionResult<Proveedor>> PostProveedor(Proveedor proveedor)
        {
            return await HandleDbCreate<Proveedor>(
                proveedor,
                async () =>
                {
                    _context.Proveedores.Add(proveedor);
                    await _context.SaveChangesAsync();
                },
                "Error creating proveedor"
            );
        }

        // DELETE: api/Proveedores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProveedor(int id)
        {
            return await HandleDbDelete<Proveedor>(
                async () => await _context.Proveedores.FindAsync(id),
                async (proveedor) =>
                {
                    _context.Proveedores.Remove(proveedor);
                    await _context.SaveChangesAsync();
                },
                $"Error deleting proveedor with ID {id}"
            );
        }
    }
}
