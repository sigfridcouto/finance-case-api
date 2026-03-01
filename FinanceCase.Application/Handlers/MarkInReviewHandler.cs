using FinanceCase.Application.Abstractions;
using FinanceCase.Application.Commands;
using FinanceCase.Application.Dto;
using FinanceCase.Application.Mapping;
using FinanceCase.Domain.Exceptions;

namespace FinanceCase.Application.Handlers;

public class MarkInReviewHandler : IMarkInReviewHandler
{
    private readonly IFinanceCaseRepository _repo;

    public MarkInReviewHandler(IFinanceCaseRepository repo) => _repo = repo;

    public async Task<CaseDto> Handle(MarkInReviewRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetAsync(request.CaseId, ct)
            ?? throw new DomainException("Case not found.");

        entity.MarkInReview();

        await _repo.SaveChangesAsync(ct);
        return CaseMapper.ToDto(entity);
    }
}