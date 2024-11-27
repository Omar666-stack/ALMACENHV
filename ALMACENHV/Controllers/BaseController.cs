using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ALMACENHV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;

        protected BaseController(ILogger logger)
        {
            _logger = logger;
        }

        protected async Task<ActionResult<T>> HandleDbOperation<T>(Func<Task<T>> operation, string errorMessage = "Error en la operación")
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
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos: {Message}", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    "Error al actualizar la base de datos: " + ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error general: {Message}", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, 
                    errorMessage + ": " + ex.Message);
            }
        }
    }
}
