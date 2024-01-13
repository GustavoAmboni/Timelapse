using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Timelapse.CLI.Application.ApplicationServices.Interfaces;
using Timelapse.CLI.Entities;
using Timelapse.CLI.Infraestructure.Data.Context;

namespace Timelapse.CLI.Application.ApplicationServices
{
    public class ItemService : IItemService
    {
        private readonly ApplicationDbContext _context;
        public ItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Item>> GetAsNoTracking(Expression<Func<Item, bool>> predicate, CancellationToken ct)
        {
            return await _context.Items
                .Include(i => i.Periods)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Item?> Get(string name, CancellationToken ct)
        {
            return await _context.Items
                .Where(w => w.Name == name)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<Item?> GetAsNoTracking(string name, CancellationToken ct)
        {
            return await _context.Items
                .Include(i => i.Periods)
                .Where(w => w.Name == name)
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);
        }

        public async Task<ICollection<Item>> Get(CancellationToken ct)
        {
            return await _context.Items
                .ToListAsync(ct);
        }

        public async Task<ICollection<Item>> Get(Expression<Func<Item, bool>> predicate, CancellationToken ct)
        {
            return await _context.Items
                .Where(predicate)
                .ToListAsync(ct);
        }

        public async ValueTask<bool> Exists(string name, CancellationToken ct)
        {
            return await _context.Items
                .Where(w => w.Name == name)
                .AsNoTracking()
                .AnyAsync();
        }

        public async Task<Item> Add(Item entity, CancellationToken ct)
        {
            _context.Add(entity);

            await _context.SaveChangesAsync(ct);

            return (await Get(entity.Name, ct))!;
        }

        public async Task<Item> Update(Item entity, CancellationToken ct)
        {
            _context.Update(entity);

            await _context.SaveChangesAsync(ct);

            return entity;
        }

        public async ValueTask<bool> IsRunning(string name, CancellationToken ct)
        {
            var item = await _context.Periods
                .AsNoTracking()
                .Where(w => w.Item.Name == name)
                .OrderBy(o => o.PeriodId)
                .LastOrDefaultAsync(ct);

            return item?.StoppedAt is null;
        }

        public async Task Remove(string name, CancellationToken ct)
        {
            var item = await Get(name, ct) ?? throw new Exception("Item not found");

            _context.Remove(item);
            await _context.SaveChangesAsync(ct);
        }
    }
}
