using FinanceCase.Application.Dto;

namespace FinanceCase.Application.Commands;

public record MarkInReviewRequest(Guid CaseId);

public interface IMarkInReviewHandler
{
    Task<CaseDto> Handle(MarkInReviewRequest request, CancellationToken ct);
}