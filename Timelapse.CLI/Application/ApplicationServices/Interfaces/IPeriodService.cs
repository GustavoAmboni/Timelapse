using System.Linq.Expressions;
using Timelapse.CLI.Entities;

namespace Timelapse.CLI.Application.ApplicationServices.Interfaces
{
    public interface IPeriodService
    {
        Task<Period> Add(Period item, CancellationToken ct);
        Task<IEnumerable<Period>> Get(CancellationToken ct);
        Task<IEnumerable<Period>> Get(Expression<Func<Period, bool>> predicate, CancellationToken ct);
        Task<Period?> Get(int id, CancellationToken ct);
        Task<IEnumerable<Period>> GetAsNoTracking(Expression<Func<Period, bool>> predicate, CancellationToken ct);
        Task<IEnumerable<Period>> GetAsNoTracking(int Take, CancellationToken ct);
        Task Remove(int id, CancellationToken ct);
        Task<Period> Update(Period item, CancellationToken ct);
        Task<Period?> GetLastRunningPeriod(CancellationToken ct);
    }
}