using MassTransit;
using TractorRental.Application.Commands;
using TractorRental.Infrastructure;
using TractorRental.IoTWorker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

// 1. Liga as camadas da arquitetura
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegistrarTelemetriaCommand).Assembly));

// 2. Configura o MassTransit para usar o RabbitMQ
builder.Services.AddMassTransit(x =>
{
    // Registra o nosso ouvinte
    x.AddConsumer<TelemetriaConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        // 🌟 A MÁGICA ENTRA AQUI: Ensina o MassTransit a ler JSON puro, sem envelope
        cfg.UseRawJsonSerializer();

        // Configura a fila "telemetria-tratores" e amarra ao nosso Consumer
        cfg.ReceiveEndpoint("telemetria-tratores", e =>
        {
            e.ConfigureConsumer<TelemetriaConsumer>(context);
        });
    });
});
var host = builder.Build();
host.Run();