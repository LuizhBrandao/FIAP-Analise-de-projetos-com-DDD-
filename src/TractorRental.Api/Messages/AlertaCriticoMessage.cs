namespace TractorRental.Messages; // <-- Mude de TractorRental.Api.Messages para este

public record AlertaCriticoMessage(Guid TratorId, double Temperatura, string Mensagem);