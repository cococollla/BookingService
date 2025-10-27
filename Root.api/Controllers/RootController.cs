using Common.Api.Results;
using LinqToDB;
using Localization.shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Root.dal;
using static Common.Api.Results.ResultsExtensions;

namespace Root.api.Controllers;

/// <summary>
/// Приложение
/// </summary>
[ApiController]
[Route("api/")]
[Tags("root")]
public class RootController : ControllerBase
{
    private readonly ILogger<RootController> _logger;
    private readonly IHostEnvironment _env;
    private readonly ILocalizer _localizer;


    /// <summary>
    /// Конструктор
    /// </summary>
    public RootController(ILogger<RootController> logger, IHostEnvironment env, ILocalizer localizer)
    {
        _logger = logger;
        _env = env;
        _localizer = localizer;
    }

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
        if (!_env.IsDevelopment() && !_env.IsEnvironment("DevelopmentContainer"))
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
        // TODO: потенциально более сложная проверка
        return ok(HealthStatus.Healthy);
    }
    
    /// <summary>
    /// Локализация 
    /// </summary>
    [HttpGet("localization")]
    public Ok<LocalizationResponse> Localization()
    {
        return ok(new LocalizationResponse
        {
            Language = _localizer.GetLanguage()
        });
    }

    /// <summary>
    /// Локализация
    /// </summary>
    public sealed record LocalizationResponse
    {
        /// <summary>
        /// Язык
        /// </summary>
        public required Language Language { get; init; }
    }
}
