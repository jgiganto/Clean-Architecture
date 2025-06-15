// --- PASO 1: CREAR LA CLASE DEL BEHAVIOR ---
// --- Proyecto: MiTienda.Application ---
// Puedes crear una carpeta "Common/Behaviors" para organizarlo.

using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MiTienda.Application.Common.Behaviors
{
    /// <summary>
    /// Este es nuestro pipeline behavior. Interceptará todas las peticiones a MediatR.
    /// Implementa IPipelineBehavior<TRequest, TResponse>, donde TRequest es la solicitud
    /// y TResponse es la respuesta.
    /// </summary>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse> // Restricción para asegurar que es una petición de MediatR
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            // --- Lógica ANTES de llamar al handler ---
            _logger.LogInformation("Iniciando Request: {Name} {@Request}", requestName, request);

            var stopwatch = Stopwatch.StartNew();

            // --- Llamada al siguiente eslabón de la cadena ---
            // 'next()' puede ser otro behavior o, si es el último, el propio Handler.
            var response = await next();

            // --- Lógica DESPUÉS de llamar al handler s ---
            stopwatch.Stop();
            _logger.LogInformation("Request Completado: {Name}; Duración: {Duration}ms", requestName, stopwatch.ElapsedMilliseconds);

            return response;
        }
    }
}
