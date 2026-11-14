using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ZenithSky_Proyecto.Common;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, title, detail) = exception switch
        {
            NotFoundException e   => (StatusCodes.Status404NotFound, "Recurso no encontrado", e.Message),
            ConflictException e   => (StatusCodes.Status409Conflict, "Conflicto", e.Message),
            BadRequestException e => (StatusCodes.Status400BadRequest, "Solicitud inválida", e.Message),
            DbUpdateException     => (StatusCodes.Status409Conflict, "Conflicto de integridad",
                                      "La operación viola una restricción de integridad de la base de datos."),
            _                     => (StatusCodes.Status500InternalServerError, "Error interno del servidor",
                                      "Ocurrió un error inesperado.")
        };

        if (status == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Error no controlado en {Path}", httpContext.Request.Path);
        else
            logger.LogWarning("{Title}: {Detail}", title, detail);

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        }, cancellationToken);

        return true;
    }
}
