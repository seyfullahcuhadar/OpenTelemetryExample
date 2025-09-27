using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;
using Microsoft.EntityFrameworkCore;
using OpenTelemetryExample.Data;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var serviceName = "OpenTelemetryExample";
var tracesEndpoint = builder.Configuration["OpenTelemetry:TracesEndpoint"] ?? "http://localhost:4318/v1/traces";
var metricsEndpoint = builder.Configuration["OpenTelemetry:MetricsEndpoint"] ?? "http://localhost:4318/v1/metrics";
var logsEndpoint = builder.Configuration["OpenTelemetry:LogsEndpoint"] ?? "http://localhost:4318/v1/logs";
builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName: serviceName))
        .AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Endpoint = new Uri(logsEndpoint);
            otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
        });
});
builder.Services.AddOpenTelemetry()
      .ConfigureResource(resource => resource.AddService(serviceName: serviceName))
      .WithTracing(tracing => tracing
          .SetSampler(new OpenTelemetry.Trace.AlwaysOnSampler())
          .AddAspNetCoreInstrumentation()
          .AddEntityFrameworkCoreInstrumentation()
          .AddNpgsql()

          .AddHttpClientInstrumentation()
          .AddOtlpExporter(otlpOptions =>
          {
              otlpOptions.Endpoint = new Uri(tracesEndpoint);
              otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
          }))
      .WithMetrics(metrics => metrics
          .AddAspNetCoreInstrumentation()
          .AddHttpClientInstrumentation()
          .AddOtlpExporter(otlpOptions =>
          {
              otlpOptions.Endpoint = new Uri(metricsEndpoint);
              otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
          }));

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();