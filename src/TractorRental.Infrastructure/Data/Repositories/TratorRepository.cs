using Microsoft.EntityFrameworkCore;
using TractorRental.Application.Interfaces;
using TractorRental.Domain.Aggregates;

namespace TractorRental.Infrastructure.Data.Repositories;

public class TratorRepository(TractorRentalDbContext context) : ITratorRepository
{
    public async Task<Trator?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Tratores
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task SalvarAsync(Trator trator, CancellationToken cancellationToken)
    {
        await context.Tratores.AddAsync(trator, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AtualizarAsync(Trator trator, CancellationToken cancellationToken)
    {
        context.Tratores.Update(trator);
        await context.SaveChangesAsync(cancellationToken);
    }
}