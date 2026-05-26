using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TractorRental.Domain.Aggregates;

namespace TractorRental.Infrastructure.Data.Configurations;

public class TratorConfiguration : IEntityTypeConfiguration<Trator>
{
    public void Configure(EntityTypeBuilder<Trator> builder)
    {
        builder.ToTable("Tratores");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Modelo)
            .IsRequired()
            .HasMaxLength(100);

        // Salva o Enum como string no banco de dados para facilitar a leitura manual
        builder.Property(t => t.Status)
            .HasConversion<string>()
            .IsRequired();

        // IMPORTANTE: Ignora a lista de eventos de domínio na persistência
        builder.Ignore(t => t.DomainEvents);
    }
}