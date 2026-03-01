using FinanceCase.Application.Dto;

namespace FinanceCase.Application.Commands;

public record ApproveCaseRequest(Guid CaseId);

public interface IApproveCaseHandler
{
    Task<CaseDto> Handle(ApproveCaseRequest request, CancellationToken ct);
}