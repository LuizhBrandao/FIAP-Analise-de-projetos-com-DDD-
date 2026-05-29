using TractorRental.Api.Endpoints;
using TractorRental.Application.Commands; // <-- Adicionado para referenciar a Application
using TractorRental.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

// LIGA O DISJUNTOR DO MEDIATR: Ensina a API a procurar os Commands e Policies na camada de Application
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegistrarTelemetriaCommand).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Tractor Rental API"));
}

app.UseHttpsRedirection();
app.MapTratorEndpoints();

app.MapGet("/", () => "Tractor Rental API Rodando com DDD e Clean Architecture!");

app.Run();