using TractorRental.Application.Queries;

namespace TractorRental.Application.Interfaces;

public interface ITratorQueries
{
    Task<IEnumerable<TratorDto>> ObterDashboardTratoresAsync();
}