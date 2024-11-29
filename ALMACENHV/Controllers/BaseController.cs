using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Data;
using System.Net;

namespace ALMACENHV.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly AlmacenContext _context;
        protected readonly ILogger _logger;

        public BaseController(AlmacenContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        protected async Task<ActionResult<IEnumerable<T>>> HandleDbOperationList<T>(Func<Task<List<T>>> operation)
        {
            try
            {
                var result = await operation();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing database operation");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error interno del servidor");
            }
        }

        protected async Task<ActionResult<T>> HandleDbOperation<T>(Func<Task<T?>> operation) where T : class
        {
            try
            {
                var result = await operation();
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing database operation");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error interno del servidor");
            }
        }

        protected async Task<IActionResult> HandleDbUpdate<T>(T entity, Func<Task> operation) where T : class
        {
            try
            {
                await operation();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing database operation");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error interno del servidor");
            }
        }

        protected async Task<ActionResult<T>> HandleDbCreate<T>(T entity, Func<Task> operation) where T : class
        {
            try
            {
                await operation();
                return CreatedAtAction("Get", new { id = GetEntityId(entity) }, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing database operation");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error interno del servidor");
            }
        }

        protected async Task<IActionResult> HandleDbDelete<T>(T entity, Func<Task> operation) where T : class
        {
            try
            {
                await operation();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing database operation");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error interno del servidor");
            }
        }

        private int GetEntityId<T>(T entity) where T : class
        {
            var property = entity.GetType().GetProperty($"{entity.GetType().Name}ID");
            return (int)(property?.GetValue(entity) ?? 0);
        }
    }
}
