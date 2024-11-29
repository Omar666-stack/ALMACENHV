using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ALMACENHV.Middleware
{
    public class ValidationExceptionMiddleware : IExceptionFilter
    {
        private readonly ILogger<ValidationExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ValidationExceptionMiddleware(ILogger<ValidationExceptionMiddleware> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException validationException)
            {
                var problemDetails = new ValidationProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = validationException.Message,
                    Instance = context.HttpContext.Request.Path
                };

                foreach (var error in validationException.Errors)
                {
                    problemDetails.Errors.Add(error.Key, new[] { error.Value });
                }

                context.Result = new BadRequestObjectResult(problemDetails);
                context.ExceptionHandled = true;

                _logger.LogWarning("Validation error occurred: {Message}", validationException.Message);
            }
        }
    }

    public class ValidationException : Exception
    {
        public IDictionary<string, string> Errors { get; }

        public ValidationException(string message, IDictionary<string, string> errors) 
            : base(message)
        {
            Errors = errors;
        }
    }
}
