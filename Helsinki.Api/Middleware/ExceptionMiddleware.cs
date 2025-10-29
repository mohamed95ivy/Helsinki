using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Helsinki.Api.Middleware
{
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ProblemDetailsFactory _pdf;
        private readonly ILogger<ExceptionMiddleware> _log;

        public ExceptionMiddleware(RequestDelegate next, ProblemDetailsFactory pdf, ILogger<ExceptionMiddleware> log)
        {
            _next = next;
            _pdf = pdf;
            _log = log;
        }

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Unhandled exception");

                var status = ex switch
                {
                    ArgumentException or ArgumentOutOfRangeException or ValidationException => StatusCodes.Status400BadRequest,
                    KeyNotFoundException => StatusCodes.Status400BadRequest,
                    HttpRequestException => StatusCodes.Status503ServiceUnavailable,
                    OperationCanceledException when !ctx.RequestAborted.IsCancellationRequested => StatusCodes.Status504GatewayTimeout,
                    _ => StatusCodes.Status500InternalServerError
                };

                var pd = _pdf.CreateProblemDetails(
                    ctx,
                    statusCode: status,
                    title: "An error occurred",
                    detail: ex.Message,
                    instance: ctx.Request.Path
                );

                ctx.Response.StatusCode = status;
                ctx.Response.ContentType = "application/problem+json";
                await ctx.Response.WriteAsJsonAsync(pd);
            }
        }
    }
}
