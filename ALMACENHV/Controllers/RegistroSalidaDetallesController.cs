using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroSalidaDetallesController : BaseController
    {
        public RegistroSalidaDetallesController(AlmacenContext context, ILogger<RegistroSalidaDetallesController> logger)
            : base(context, logger)
        {
        }

        // GET: api/RegistroSalidaDetalles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroSalidaDetalle>>> GetRegistroSalidaDetalles()
        {
            return await HandleDbOperationList(
                () => _context.RegistroSalidaDetalles
                    .Include(r => r.RegistroSalida)
                    .Include(r => r.Producto)
                    .ToListAsync()
            );
        }

        // GET: api/RegistroSalidaDetalles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroSalidaDetalle>> GetRegistroSalidaDetalle(int id)
        {
            return await HandleDbOperation(
                () => _context.RegistroSalidaDetalles
                    .Include(r => r.RegistroSalida)
                    .Include(r => r.Producto)
                    .FirstOrDefaultAsync(r => r.Id == id)
            );
        }

        // POST: api/RegistroSalidaDetalles
        [HttpPost]
        public async Task<ActionResult<RegistroSalidaDetalle>> PostRegistroSalidaDetalle(RegistroSalidaDetalle registroSalidaDetalle)
        {
            return await HandleDbCreate(
                registroSalidaDetalle,
                async () => {
                    await _context.RegistroSalidaDetalles.AddAsync(registroSalidaDetalle);
                    await _context.SaveChangesAsync();
                },
                "Crear detalle de salida"
            );
        }

        // PUT: api/RegistroSalidaDetalles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroSalidaDetalle(int id, RegistroSalidaDetalle registroSalidaDetalle)
        {
            if (id != registroSalidaDetalle.Id)
            {
                return BadRequest();
            }

            return await HandleDbUpdate(
                registroSalidaDetalle,
                async () => {
                    _context.Entry(registroSalidaDetalle).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                },
                "Actualizar detalle de salida"
            );
        }

        // DELETE: api/RegistroSalidaDetalles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistroSalidaDetalle(int id)
        {
            return await HandleDbDelete<RegistroSalidaDetalle>(
                async () => await _context.RegistroSalidaDetalles.FindAsync(id),
                async (detalle) => {
                    _context.RegistroSalidaDetalles.Remove(detalle);
                    await _context.SaveChangesAsync();
                },
                "Eliminar detalle de salida"
            );
        }
    }
}
