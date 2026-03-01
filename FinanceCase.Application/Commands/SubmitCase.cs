using FinanceCase.Application.Dto;

namespace FinanceCase.Application.Commands;

public record SubmitCaseRequest(Guid CaseId);

public interface ISubmitCaseHandler
{
    Task<CaseDto> Handle(SubmitCaseRequest request, CancellationToken ct);
}