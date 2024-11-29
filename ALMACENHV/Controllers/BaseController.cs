using Microsoft.AspNetCore.Mvc;
using ALMACENHV.Models;
using ALMACENHV.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

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

        protected async Task<ActionResult<IEnumerable<T>>> HandleDbOperationList<T>(
            Func<Task<List<T>>> operation,
            string errorMessage = "Error al procesar la operación") where T : class
        {
            try
            {
                var result = await operation();
                if (result == null || !result.Any())
                {
                    return NotFound();
                }
                return Ok(result.AsEnumerable());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, errorMessage);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        protected async Task<ActionResult<T>> HandleDbOperation<T>(
            Func<Task<T?>> operation,
            string errorMessage = "Error al procesar la operación") where T : class
        {
            try
            {
                var result = await operation();
                if (result == null)
                {
                    _logger.LogWarning("No se encontró el recurso solicitado");
                    return NotFound("El recurso solicitado no existe");
                }
                return Ok(result);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"{errorMessage} - Error de actualización de base de datos");
                return StatusCode(500, "Error al actualizar la base de datos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{errorMessage} - Error interno");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        protected async Task<ActionResult<T>> HandleDbCreate<T>(
            T entity,
            Func<Task> operation,
            string errorMessage = "Error al crear el registro") where T : class
        {
            try
            {
                await operation();
                var entityId = GetEntityId(entity);
                return CreatedAtAction(nameof(GetById), new { id = entityId }, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, errorMessage);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        protected async Task<IActionResult> HandleDbUpdate<T>(
            T entity,
            Func<Task> operation,
            string errorMessage = "Error al actualizar el registro") where T : class
        {
            try
            {
                await operation();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, errorMessage);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        protected async Task<IActionResult> HandleDbDelete<T>(
            Func<Task<T?>> findOperation,
            Func<T, Task> deleteOperation,
            string errorMessage = "Error al eliminar el registro") where T : class
        {
            try
            {
                var entity = await findOperation();
                if (entity == null)
                {
                    return NotFound();
                }

                await deleteOperation(entity);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, errorMessage);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        private static object? GetEntityId<T>(T entity) where T : class
        {
            var type = typeof(T);
            
            // Primero intentamos obtener la propiedad Id
            var idProperty = type.GetProperty("Id");
            if (idProperty != null)
            {
                return idProperty.GetValue(entity);
            }

            // Si no encontramos Id, buscamos la propiedad [Type]ID
            idProperty = type.GetProperty($"{type.Name}ID");
            if (idProperty != null)
            {
                return idProperty.GetValue(entity);
            }

            // Si no encontramos ninguna, buscamos cualquier propiedad que termine en ID
            idProperty = type.GetProperties()
                .FirstOrDefault(p => p.Name.EndsWith("ID", StringComparison.OrdinalIgnoreCase));
            
            return idProperty?.GetValue(entity);
        }

        [NonAction]
        public virtual async Task<ActionResult<T>> GetById<T>(int id) where T : class
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
    }
}
