using FinanceCase.Domain.Entities;

namespace FinanceCase.Application.Abstractions;

public interface IFinanceCaseRepository
{
    Task<FinanceCase.Domain.Entities.FinanceCase?> GetAsync(Guid id, CancellationToken ct);
    Task AddAsync(FinanceCase.Domain.Entities.FinanceCase financeCase, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}