using MassTransit;
using MediatR;
using TractorRental.Application.Commands;
using TractorRental.IoTWorker.Messages;

namespace TractorRental.IoTWorker.Consumers;

public class TelemetriaConsumer(
    IMediator mediator,
    ILogger<TelemetriaConsumer> logger) : IConsumer<TelemetriaMessage>
{
    public async Task Consume(ConsumeContext<TelemetriaMessage> context)
    {
        var mensagem = context.Message;
        logger.LogInformation("🐇 [RabbitMQ] Recebida telemetria do trator: {TratorId}", mensagem.TratorId);

        // 1. Transforma a mensagem externa no Comando interno da nossa aplicação
        var command = new RegistrarTelemetriaCommand(
            mensagem.TratorId,
            mensagem.TemperaturaMotor,
            mensagem.PressaoPneus,
            mensagem.NivelCombustivel
        );

        // 2. Envia para a camada de Application via MediatR (que já testamos e sabemos que funciona)
        var sucesso = await mediator.Send(command);

        if (sucesso)
            logger.LogInformation("✅ Telemetria processada e salva no banco com sucesso.");
        else
            logger.LogWarning("⚠️ Trator {TratorId} não encontrado. Mensagem descartada.", mensagem.TratorId);
    }
}