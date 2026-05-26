using MediatR;
using TractorRental.Application.Interfaces;

namespace TractorRental.Application.Commands;

public class RegistrarTelemetriaCommandHandler(
    ITratorRepository repository,
    IMediator mediator) : IRequestHandler<RegistrarTelemetriaCommand, bool>
{
    public async Task<bool> Handle(RegistrarTelemetriaCommand request, CancellationToken cancellationToken)
    {
        // 1. Carrega o Agregado do banco de dados
        var trator = await repository.ObterPorIdAsync(request.TratorId, cancellationToken);

        if (trator is null)
            return false;

        // 2. Delega a regra de negócio para o Agregado (DDD puro)
        trator.ProcessarLeituraSensores(request.TemperaturaMotor, request.PressaoPneus, request.NivelCombustivel);

        // 3. Persiste o novo estado
        await repository.AtualizarAsync(trator, cancellationToken);

        // 4. Dispara os eventos de domínio (fatos) gerados pelo Agregado para as Policies escutarem
        foreach (var domainEvent in trator.DomainEvents)
        {
            // Publish notifica todos os ouvintes desse evento simultaneamente
            await mediator.Publish(domainEvent, cancellationToken);
        }

        // Limpa a fila de eventos após o disparo
        trator.LimparEventos();

        return true;
    }
}