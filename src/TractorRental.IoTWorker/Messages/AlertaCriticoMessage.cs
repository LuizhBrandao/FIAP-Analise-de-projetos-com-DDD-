namespace TractorRental.Messages; // <-- Mude de TractorRental.IoTWorker.Messages para este

public record AlertaCriticoMessage(Guid TratorId, double Temperatura, string Mensagem);