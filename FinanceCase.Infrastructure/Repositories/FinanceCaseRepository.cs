using FinanceCase.Application.Abstractions;
using FinanceCase.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace FinanceCase.Infrastructure.Repositories;

public class FinanceCaseRepository : IFinanceCaseRepository
{
    private readonly AppDbContext _db;

    public FinanceCaseRepository(AppDbContext db) => _db = db;

    public Task<FinanceCase.Domain.Entities.FinanceCase?> GetAsync(Guid id, CancellationToken ct) =>
        _db.FinanceCases
           .Include(c => c.Customer)
           .Include("Notes")
           .FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task AddAsync(FinanceCase.Domain.Entities.FinanceCase financeCase, CancellationToken ct) =>
        _db.FinanceCases.AddAsync(financeCase, ct).AsTask();

    public Task SaveChangesAsync(CancellationToken ct) =>
        _db.SaveChangesAsync(ct);
}