using TractorRental.Domain.Aggregates;

namespace TractorRental.Application.Interfaces;

public interface ITratorRepository
{
    Task<Trator?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken);
    Task SalvarAsync(Trator trator, CancellationToken cancellationToken);
    Task AtualizarAsync(Trator trator, CancellationToken cancellationToken);
}