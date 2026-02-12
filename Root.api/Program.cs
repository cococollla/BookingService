using Booking.app;
using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Api;
using Common.Api.Swagger;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using Root.dal;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 📡 Add services to the container.
builder.Services.AddControllers();
builder.Services.AddBookingModule();

var dbConnectionString = builder.Configuration.GetConnectionString("Default") ?? string.Empty;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

#region 📚 API DOCUMENTATION

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// TODO: пока нет чтения xml документации https://devblogs.microsoft.com/dotnet/dotnet9-openapi/ искать по ключу xml
// builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(opt =>
{
    // TODO: потенциально можно настроить несколько версий api и для каждой версии свой документ.
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BOOKING API",
        Description = "Полное описание api",
    });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    opt.UseDefaultConfig();
});

// JSON ENUM FOR IRESULT
// Есть проблема и пока только такое решение.
// https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/2293
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    // Это что бы IResults enmus сериализовались.
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(namingPolicy: JsonNamingPolicy.CamelCase));
}).Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    // Это что бы swagger видел настройки сериализации.
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(namingPolicy: JsonNamingPolicy.CamelCase));
});

#endregion

#region 📦 DB

// TODO: контекстов возможно будет несколько, аккуратно нужно их конфигурировать.
builder.Services.AddLinqToDBContext<Rootdb>((provider, options)
    => options
       .UsePostgreSQL(dbConnectionString)
       .UseMappingSchema(Rootdb.Schema())
       .UseDefaultLogging(provider));

#endregion

#region ❌ ERRORS

builder.Services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();
builder.Services.AddProblemDetails();

#endregion

#region 📜 LOGS

builder.Services.AddHttpContextAccessor(); // это для correlation-id (https://github.com/ekmsystems/serilog-enrichers-correlation-id)
builder.Host
       .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

#endregion


#region 🔄 PIPLINE

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("DevelopmentContainer")
                                    || app.Environment.IsEnvironment("DevelopmentContainerFrontend"))
{
    // TODO: пока не используем
    // пока нет чтения xml документации https://devblogs.microsoft.com/dotnet/dotnet9-openapi/ искать по ключу xml
    // app.MapOpenApi();
    app.UseSwagger(options => { options.RouteTemplate = "/openapi/{documentName}.json"; });
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "booking"); });
    app.MapScalarApiReference();
}

// ❌ ERRORS
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

#endregion

app.Run();