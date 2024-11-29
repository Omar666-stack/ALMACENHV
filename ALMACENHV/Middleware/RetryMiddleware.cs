using System.Net;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ALMACENHV.Middleware
{
    public class RetryMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RetryMiddleware> _logger;
        private readonly AsyncRetryPolicy<IResult> _retryPolicy;

        public RetryMiddleware(RequestDelegate next, ILogger<RetryMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _retryPolicy = CreateRetryPolicy();
        }

        private AsyncRetryPolicy<IResult> CreateRetryPolicy()
        {
            return Policy<IResult>
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(
                            "Reintento {RetryCount} después de {Delay}s. Error: {Error}",
                            retryCount,
                            timeSpan.TotalSeconds,
                            exception.GetType().Name);
                    }
                );
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    await _next(context);
                    return Results.Ok();
                }
                catch (Exception ex) when (ShouldRetry(ex, context))
                {
                    _logger.LogError(ex, "Error en la solicitud: {Error}", ex.Message);
                    throw;
                }
            });
        }

        private bool ShouldRetry(Exception ex, HttpContext context)
        {
            if (context.Response.StatusCode >= 500)
                return true;

            return ex is HttpRequestException || 
                   ex is TimeoutException ||
                   ex is DbUpdateException;
        }
    }

    public static class RetryMiddlewareExtensions
    {
        public static IApplicationBuilder UseRetryMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RetryMiddleware>();
        }
    }
}
