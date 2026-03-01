using FinanceCase.Domain.Enums;

namespace FinanceCase.Application.Dto;

public record CaseDto(
    Guid Id,
    Guid CustomerId,
    CaseStatus Status,
    decimal RequestedAmount,
    int TermMonths,
    decimal AnnualIncome,
    int RiskScore,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    IReadOnlyList<CaseNoteDto> Notes
);

public record CaseNoteDto(DateTimeOffset CreatedAt, string Message);