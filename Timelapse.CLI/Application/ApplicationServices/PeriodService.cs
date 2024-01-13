﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Timelapse.CLI.Application.ApplicationServices.Interfaces;
using Timelapse.CLI.Entities;
using Timelapse.CLI.Infraestructure.Data.Context;

namespace Timelapse.CLI.Application.ApplicationServices
{
    public class PeriodService : IPeriodService
    {
        private readonly ApplicationDbContext _context;

        public PeriodService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Period>> GetAsNoTracking(Expression<Func<Period, bool>> predicate, CancellationToken ct)
        {
            return await _context.Periods
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Period?> Get(int id, CancellationToken ct)
        {
            return await _context.Periods
                .Where(w => w.PeriodId == id)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<ICollection<Period>> Get(CancellationToken ct)
        {
            return await _context.Periods
                .ToListAsync(ct);
        }

        public async Task<ICollection<Period>> Get(Expression<Func<Period, bool>> predicate, CancellationToken ct)
        {
            return await _context.Periods
                .Where(predicate)
                .ToListAsync(ct);
        }

        public async Task<Period> Add(Period entity, CancellationToken ct)
        {
            _context.Add(entity);

            await _context.SaveChangesAsync(ct);

            return (await Get(entity.PeriodId, ct))!;
        }

        public async Task<Period> Update(Period entity, CancellationToken ct)
        {
            _context.Update(entity);

            await _context.SaveChangesAsync(ct);

            return entity;
        }

        public async Task Remove(int id, CancellationToken ct)
        {
            var item = await Get(id, ct) ?? throw new Exception("Not found");

            _context.Remove(item);
            await _context.SaveChangesAsync(ct);
        }
    }
}
