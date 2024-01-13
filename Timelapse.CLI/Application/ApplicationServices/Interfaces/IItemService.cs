using System.Linq.Expressions;
using Timelapse.CLI.Entities;

namespace Timelapse.CLI.Application.ApplicationServices.Interfaces
{
    public interface IItemService
    {
        Task<Item> Add(Item item, CancellationToken ct);
        Task<ICollection<Item>> Get(CancellationToken ct);
        Task<ICollection<Item>> Get(Expression<Func<Item, bool>> predicate, CancellationToken ct);
        Task<Item?> Get(string name, CancellationToken ct);
        Task<ICollection<Item>> GetAsNoTracking(Expression<Func<Item, bool>> predicate, CancellationToken ct);
        Task<Item?> GetAsNoTracking(string name, CancellationToken ct);
        Task Remove(string name, CancellationToken ct);
        Task<Item> Update(Item item, CancellationToken ct);
        ValueTask<bool> Exists(string name, CancellationToken ct);
        ValueTask<bool> IsRunning(string name, CancellationToken ct);
    }
}