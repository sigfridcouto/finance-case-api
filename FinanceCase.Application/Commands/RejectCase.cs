using FinanceCase.Application.Dto;

namespace FinanceCase.Application.Commands;

public record RejectCaseRequest(Guid CaseId, string Reason);

public interface IRejectCaseHandler
{
    Task<CaseDto> Handle(RejectCaseRequest request, CancellationToken ct);
}