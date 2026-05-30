using MassTransit;
using TractorRental.Infrastructure.Data;
using TractorRental.Messages;

namespace TractorRental.Api.Services;

public class SensorSimulatorWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<SensorSimulatorWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("🚜 Robô de simulação de sensores iniciado!");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(5000, stoppingToken);

            try
            {
                using var scope = scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<TractorRentalDbContext>();
                var bus = scope.ServiceProvider.GetRequiredService<IBus>();

                var trator = dbContext.Tratores.FirstOrDefault();

                if (trator is not null)
                {
                    var random = new Random();

                    // 🌟 CORREÇÃO: Agora usamos o nosso tipo concreto em vez de um tipo anônimo
                    var telemetria = new TelemetriaMessage(
                        trator.Id,
                        80.0 + (random.NextDouble() * 40.0),
                        30.0 + (random.NextDouble() * 5.0),
                        random.Next(10, 100)
                    );

                    await bus.Publish(telemetria);

                    logger.LogInformation("📡 Telemetria enviada para o Trator {TratorId} | Temp: {Temp:F1}ºC", trator.Id, telemetria.TemperaturaMotor);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao simular telemetria.");
            }
        }
    }
}