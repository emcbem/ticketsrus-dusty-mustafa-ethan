using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Text.Json.Serialization;
using TicketClassLib.Services;
using TicketWebApp.Data;
using TicketWebApp.Services;
using TicketWebApp.Components;
using OpenTelemetry.Logs;
using System.Diagnostics.Tracing;
using TicketWebApp.Telemetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


builder.Services.AddSingleton<ITicketService, ApiTicketService>();
builder.Services.AddSingleton<IEventService, ApiEventService>();
builder.Services.AddDbContextFactory<PostgresContext>(optionsBuilder => optionsBuilder.UseNpgsql("Name=TicketsDB"));
builder.Services.AddScoped<EmailSender>();
builder.Services.AddHealthChecks();
builder.Services.AddLogging();

const string serviceName = "thingy";
string otelString = builder.Configuration["COLLECTOR_URL"] ?? throw new NullReferenceException("Environment variable not set: COLLECTOR_URL");

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
        .AddOtlpExporter(o =>
        {
            o.Endpoint = new Uri(otelString);
        })
       .AddConsoleExporter()
       ;
});


builder.Services.AddOpenTelemetry()
     .ConfigureResource(resource => resource.AddService(EthanTraces.Name))
     .WithTracing(tracing => tracing
         .AddSource(EthanTraces.Name)
         .AddAspNetCoreInstrumentation()
         //.AddConsoleExporter()
         .AddOtlpExporter(o =>
           o.Endpoint = new Uri(otelString)))
     .WithMetrics(metrics => metrics
         .AddAspNetCoreInstrumentation()
         .AddMeter(EthanMetrics.Meter.Name)
         // .AddConsoleExporter()
         .AddOtlpExporter(o =>
           o.Endpoint = new Uri(otelString)))
    .ConfigureResource(res => res.AddService("NewOne"))
    .WithTracing(t => t
         .AddSource(EthanSecondTraces.Name)
         .AddAspNetCoreInstrumentation()
         //.AddConsoleExporter()
         .AddOtlpExporter(o =>
           o.Endpoint = new Uri(otelString)));

builder.Services.AddAntiforgery(options => { options.Cookie.Expiration = TimeSpan.Zero; });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.MapHealthChecks("/healthCheck", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        }
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor API V1");
});

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapControllers();
app.Run();

public partial class Program { }