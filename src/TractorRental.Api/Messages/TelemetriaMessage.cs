namespace TractorRental.Messages;

public record TelemetriaMessage(
    Guid TratorId,
    double TemperaturaMotor,
    double PressaoPneus,
    double NivelCombustivel
);
