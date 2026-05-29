namespace TractorRental.IoTWorker.Messages;

// Este é o formato exato que a mensagem JSON deve ter ao cair no RabbitMQ
public record TelemetriaMessage(
    Guid TratorId,
    double TemperaturaMotor,
    double PressaoPneus,
    double NivelCombustivel
);