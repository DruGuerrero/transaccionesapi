using Microsoft.OpenApi;
using Transacciones.API;
using Transacciones.Infrastructure;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());
builder.Services.AddAPI();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Transacciones API",
        Version     = "v1",
        Description = "API para gestionar transacciones."
    });

    options.CustomOperationIds(e =>
        $"{e.ActionDescriptor.RouteValues["controller"]}_{e.HttpMethod}");
    options.EnableAnnotations();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Transacciones API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
