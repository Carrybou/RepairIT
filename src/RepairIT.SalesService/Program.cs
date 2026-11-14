// Squelette du microservice : les fonctionnalités métier arrivent en V3 (voir README).
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "RepairIT - Ventes"));
}

app.MapHealthChecks("/health");

app.Run();
