using Common.Api.Results;
using LinqToDB;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Root.dal;
using static Common.Api.ResultsExtensions;

namespace BookingService.Controllers;

/// <summary>
/// Приложение
/// </summary>
[ApiController]
[Route("api/")]
[Tags("root")]
public class RootController(ILogger<RootController> logger, IHostEnvironment env) : ControllerBase
{
    /// <summary>
    /// Получить версию приложения
    /// </summary>
    [HttpGet("version")]
    public Ok<string> Version()
    {
        return ok("0.1.0");
    }

    /// <summary>
    /// Получить изменения в бд
    /// </summary>
    [HttpGet("db/changelog")]
    public async Task<Results<Ok<DatabaseChangeLog[]>, TNotFound>> DbChangelog([FromServices] Rootdb rootdb, CancellationToken cancellationToken)
    {
        if (!env.IsDevelopment() && !env.IsEnvironment("DevelopmentContainer"))
            return notFound("Environment is not development");

        return ok(await rootdb.DatabaseChangeLogs
            .OrderByDescending(log => log.DateExecuted)
            .ToArrayAsync(cancellationToken));
    }

    /// <summary>
    /// Healthcheck 
    /// </summary>
    [HttpGet("health")]
    public Ok<HealthStatus> Health()
    {
        return ok(HealthStatus.Healthy);
    }
}
