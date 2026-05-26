using Microsoft.EntityFrameworkCore;
using TractorRental.Domain.Aggregates;

namespace TractorRental.Infrastructure.Data;

public class TractorRentalDbContext(DbContextOptions<TractorRentalDbContext> options) : DbContext(options)
{
    public DbSet<Trator> Tratores => Set<Trator>();
    public DbSet<ContratoAluguel> ContratosAluguel => Set<ContratoAluguel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Alimenta automaticamente os mapeamentos a partir das classes que implementam IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TractorRentalDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}