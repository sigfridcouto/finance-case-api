using FinanceCase.Application.Dto;

namespace FinanceCase.Application.Commands;

public record CreateCaseRequest(Guid CustomerId, decimal RequestedAmount, int TermMonths, decimal AnnualIncome);

public interface ICreateCaseHandler
{
    Task<CaseDto> Handle(CreateCaseRequest request, CancellationToken ct);
}