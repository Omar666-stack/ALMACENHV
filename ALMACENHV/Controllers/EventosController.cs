using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using ALMACENHV.Data;

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : BaseController
    {
        private new readonly AlmacenContext _context;
        private new readonly ILogger<EventosController> _logger;

        public EventosController(AlmacenContext context, ILogger<EventosController> logger)
            : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Eventos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            return await HandleDbOperationList<Evento>(
                async () => await _context.Eventos
                    .Include(e => e.Usuario)
                    .ToListAsync(),
                "Error retrieving eventos"
            );
        }

        // GET: api/Eventos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            return await HandleDbOperation<Evento>(
                async () => await _context.Eventos
                    .Include(e => e.Usuario)
                    .FirstOrDefaultAsync(e => e.EventoID == id),
                $"Error retrieving evento with ID {id}"
            );
        }

        // PUT: api/Eventos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvento(int id, Evento evento)
        {
            if (id != evento.EventoID)
            {
                return BadRequest();
            }

            _context.Entry(evento).State = EntityState.Modified;

            return await HandleDbUpdate<Evento>(
                evento,
                async () => await _context.SaveChangesAsync(),
                $"Error updating evento with ID {id}"
            );
        }

        // POST: api/Eventos
        [HttpPost]
        public async Task<ActionResult<Evento>> PostEvento(Evento evento)
        {
            return await HandleDbCreate<Evento>(
                evento,
                async () =>
                {
                    _context.Eventos.Add(evento);
                    await _context.SaveChangesAsync();
                },
                "Error creating evento"
            );
        }

        // DELETE: api/Eventos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            return await HandleDbDelete<Evento>(
                async () => await _context.Eventos.FindAsync(id),
                async (evento) =>
                {
                    _context.Eventos.Remove(evento);
                    await _context.SaveChangesAsync();
                },
                $"Error deleting evento with ID {id}"
            );
        }
    }
}
