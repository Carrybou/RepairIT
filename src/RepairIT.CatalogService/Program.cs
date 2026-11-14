using RepairIT.CatalogService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AjouterCatalogue(builder.Configuration);

var app = builder.Build();

await app.InitialiserBaseDeDonneesAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "RepairIT - Catalogue"));
}

app.MapControllers();

app.Run();
