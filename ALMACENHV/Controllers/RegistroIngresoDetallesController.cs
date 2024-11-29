using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroIngresoDetallesController : BaseController
    {
        public RegistroIngresoDetallesController(AlmacenContext context, ILogger<RegistroIngresoDetallesController> logger)
            : base(context, logger)
        {
        }

        // GET: api/RegistroIngresoDetalles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroIngresoDetalle>>> GetRegistroIngresoDetalles()
        {
            return await HandleDbOperationList<RegistroIngresoDetalle>(
                async () => await _context.RegistroIngresoDetalles
                    .Include(r => r.RegistroIngreso)
                    .Include(r => r.Producto)
                    .ToListAsync(),
                "Error retrieving registro ingreso detalles");
        }

        // GET: api/RegistroIngresoDetalles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroIngresoDetalle>> GetRegistroIngresoDetalle(int id)
        {
            return await HandleDbOperation<RegistroIngresoDetalle>(
                async () => await _context.RegistroIngresoDetalles
                    .Include(r => r.RegistroIngreso)
                    .Include(r => r.Producto)
                    .FirstOrDefaultAsync(r => r.RegistroIngresoDetalleID == id),
                $"Error retrieving registro ingreso detalle with ID {id}");
        }

        // PUT: api/RegistroIngresoDetalles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroIngresoDetalle(int id, RegistroIngresoDetalle registroIngresoDetalle)
        {
            if (id != registroIngresoDetalle.RegistroIngresoDetalleID)
            {
                return BadRequest();
            }

            _context.Entry(registroIngresoDetalle).State = EntityState.Modified;

            return await HandleDbUpdate<RegistroIngresoDetalle>(
                registroIngresoDetalle,
                async () => await _context.SaveChangesAsync(),
                $"Error updating registro ingreso detalle with ID {id}");
        }

        // POST: api/RegistroIngresoDetalles
        [HttpPost]
        public async Task<ActionResult<RegistroIngresoDetalle>> PostRegistroIngresoDetalle(RegistroIngresoDetalle registroIngresoDetalle)
        {
            _context.RegistroIngresoDetalles.Add(registroIngresoDetalle);

            return await HandleDbCreate<RegistroIngresoDetalle>(
                registroIngresoDetalle,
                async () => await _context.SaveChangesAsync(),
                "Error creating registro ingreso detalle");
        }

        // DELETE: api/RegistroIngresoDetalles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistroIngresoDetalle(int id)
        {
            return await HandleDbDelete<RegistroIngresoDetalle>(
                async () => await _context.RegistroIngresoDetalles.FindAsync(id),
                async (detalle) => {
                    _context.RegistroIngresoDetalles.Remove(detalle);
                    await _context.SaveChangesAsync();
                },
                $"Error deleting registro ingreso detalle with ID {id}");
        }
    }
}
