using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace ALMACENHV.Extensions
{
    public static class PollyExtensions
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => 
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }

        public static AsyncRetryPolicy GetDefaultRetryPolicy()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        // Aquí podrías agregar logging si lo necesitas
                    }
                );
        }

        public static AsyncRetryPolicy GetDatabaseRetryPolicy()
        {
            return Policy
                .Handle<DbUpdateException>()
                .Or<DbUpdateConcurrencyException>()
                .WaitAndRetryAsync(
                    3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        // Aquí podrías agregar logging específico para errores de base de datos
                    }
                );
        }
    }
}
