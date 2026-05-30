using MassTransit;
using MediatR;
using TractorRental.Application.Commands;
using TractorRental.Messages;

namespace TractorRental.IoTWorker.Consumers;

public class TelemetriaConsumer(
    IMediator mediator,
    ILogger<TelemetriaConsumer> logger) : IConsumer<TelemetriaMessage>
{
    public async Task Consume(ConsumeContext<TelemetriaMessage> context)
    {
        var mensagem = context.Message;

        // 1. INSPEÇÃO: Loga tudo que chega, para sabermos se o RabbitMQ está entregando
        logger.LogInformation("🔍 Mensagem recebida no Consumer: Trator {Id}, Temp: {Temp}ºC",
            mensagem.TratorId, mensagem.TemperaturaMotor);

        try
        {
            // 2. TENTA SALVAR NO BANCO
            var command = new RegistrarTelemetriaCommand(
                mensagem.TratorId, mensagem.TemperaturaMotor,
                mensagem.PressaoPneus, mensagem.NivelCombustivel);

            var sucesso = await mediator.Send(command);

            if (sucesso)
            {
                logger.LogInformation("✅ Telemetria do trator {TratorId} salva.", mensagem.TratorId);
            }
            else
            {
                logger.LogWarning("⚠️ Falha ao salvar telemetria no banco (Regra de negócio impediu).");
            }

            // 3. O ALERTA É INDEPENDENTE: Se a temperatura for alta, dispara o alerta AGORA
            // (Mesmo que o banco tenha falhado, a sirene tem que tocar!)
            if (mensagem.TemperaturaMotor >= 110.0)
            {
                logger.LogWarning("🔥 Temperatura Crítica ({Temp:F1}ºC)! Disparando alerta via RabbitMQ...", mensagem.TemperaturaMotor);

                var endpoint = await context.GetSendEndpoint(new Uri("queue:alertas-frontend"));

                await endpoint.Send(new AlertaCriticoMessage(
                    mensagem.TratorId,
                    mensagem.TemperaturaMotor,
                    "ATENÇÃO: Risco de superaquecimento do motor!"
                ));
            }
        }
        catch (Exception ex)
        {
            // 4. ARMADILHA DE ERRO: Se o código explodir por qualquer motivo, saberemos aqui
            logger.LogError("❌ ERRO CRÍTICO AO PROCESSAR {Temp}ºC: {Erro}", mensagem.TemperaturaMotor, ex.Message);
            throw; // Relança para o MassTransit tentar novamente se necessário
        }
    }
}