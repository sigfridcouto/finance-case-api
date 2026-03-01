using FinanceCase.Application.Dto;
using FinanceCase.Domain.Entities;

namespace FinanceCase.Application.Mapping;

public static class CaseMapper
{
    public static CaseDto ToDto(FinanceCase.Domain.Entities.FinanceCase c) =>
        new(
            c.Id,
            c.CustomerId,
            c.Status,
            c.RequestedAmount,
            c.TermMonths,
            c.AnnualIncome,
            c.RiskScore,
            c.CreatedAt,
            c.UpdatedAt,
            c.Notes
                .OrderBy(n => n.CreatedAt)
                .Select(n => new CaseNoteDto(n.CreatedAt, n.Message))
                .ToList()
        );
}