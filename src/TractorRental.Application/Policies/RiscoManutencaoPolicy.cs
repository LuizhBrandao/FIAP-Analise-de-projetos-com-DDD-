using MediatR;
using TractorRental.Application.Interfaces;
using TractorRental.Domain.Events;

namespace TractorRental.Application.Policies;

// INotificationHandler diz que esta classe reage a um evento específico
public class RiscoManutencaoPolicy(
    ITratorRepository repository,
    IMediator mediator) : INotificationHandler<LeituraRecebidaEvent>
{
    public async Task Handle(LeituraRecebidaEvent notification, CancellationToken cancellationToken)
    {
        // Regra de Negócio Crítica: O motor está superaquecendo?
        if (notification.TemperaturaMotor > 110.0)
        {
            var trator = await repository.ObterPorIdAsync(notification.TratorId, cancellationToken);

            if (trator is not null)
            {
                // Dispara a ação de negócio no agregado para gerar o alerta e mudar o status
                trator.RegistrarAlertaManutencao("Risco Crítico: Temperatura do motor excedeu 110°C");

                await repository.AtualizarAsync(trator, cancellationToken);

                // Dispara o novo evento (AlertaGeradoEvent) para que outros módulos (ex: painel web) sejam atualizados
                foreach (var domainEvent in trator.DomainEvents)
                {
                    await mediator.Publish(domainEvent, cancellationToken);
                }

                trator.LimparEventos();
            }
        }
    }
}