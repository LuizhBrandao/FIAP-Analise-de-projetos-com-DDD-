using TractorRental.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o gerador de documentação da API
builder.Services.AddOpenApi();

// Aqui chamamos o método que criamos na Infraestrutura (Agora ele vai encontrar!)
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Endpoint raiz para validar que a API está de pé
app.MapGet("/", () => "Tractor Rental API Rodando com DDD e Clean Architecture!");

app.Run();