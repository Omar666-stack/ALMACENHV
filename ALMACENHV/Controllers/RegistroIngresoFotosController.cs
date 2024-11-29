using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;

namespace ALMACENHV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroIngresoFotosController : BaseController
    {
        public RegistroIngresoFotosController(AlmacenContext context, ILogger<RegistroIngresoFotosController> logger)
            : base(context, logger)
        {
        }

        // GET: api/RegistroIngresoFotos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroIngresoFoto>>> GetRegistroIngresoFotos()
        {
            return await HandleDbOperationList(
                () => _context.RegistroIngresoFotos
                    .Include(r => r.RegistroIngreso)
                    .ToListAsync()
            );
        }

        // GET: api/RegistroIngresoFotos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroIngresoFoto>> GetRegistroIngresoFoto(int id)
        {
            return await HandleDbOperation(
                () => _context.RegistroIngresoFotos
                    .Include(r => r.RegistroIngreso)
                    .FirstOrDefaultAsync(r => r.Id == id)
            );
        }

        // POST: api/RegistroIngresoFotos
        [HttpPost]
        public async Task<ActionResult<RegistroIngresoFoto>> PostRegistroIngresoFoto(RegistroIngresoFoto registroIngresoFoto)
        {
            return await HandleDbCreate(
                registroIngresoFoto,
                async () => {
                    await _context.RegistroIngresoFotos.AddAsync(registroIngresoFoto);
                    await _context.SaveChangesAsync();
                },
                "Crear foto de ingreso"
            );
        }

        // PUT: api/RegistroIngresoFotos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistroIngresoFoto(int id, RegistroIngresoFoto registroIngresoFoto)
        {
            if (id != registroIngresoFoto.Id)
            {
                return BadRequest();
            }

            return await HandleDbUpdate(
                registroIngresoFoto,
                async () => {
                    _context.Entry(registroIngresoFoto).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                },
                "Actualizar foto de ingreso"
            );
        }

        // DELETE: api/RegistroIngresoFotos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistroIngresoFoto(int id)
        {
            return await HandleDbDelete<RegistroIngresoFoto>(
                async () => await _context.RegistroIngresoFotos.FindAsync(id),
                async (foto) => {
                    _context.RegistroIngresoFotos.Remove(foto);
                    await _context.SaveChangesAsync();
                },
                "Eliminar foto de ingreso"
            );
        }
    }
}
