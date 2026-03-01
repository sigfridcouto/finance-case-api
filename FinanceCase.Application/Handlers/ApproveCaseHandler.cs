using FinanceCase.Application.Abstractions;
using FinanceCase.Application.Commands;
using FinanceCase.Application.Dto;
using FinanceCase.Application.Mapping;
using FinanceCase.Domain.Exceptions;

namespace FinanceCase.Application.Handlers;

public class ApproveCaseHandler : IApproveCaseHandler
{
    private readonly IFinanceCaseRepository _repo;

    // demo threshold; make it config later if you want
    private const int MaxRiskScore = 65;

    public ApproveCaseHandler(IFinanceCaseRepository repo) => _repo = repo;

    public async Task<CaseDto> Handle(ApproveCaseRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetAsync(request.CaseId, ct)
            ?? throw new DomainException("Case not found.");

        entity.Approve(MaxRiskScore);

        await _repo.SaveChangesAsync(ct);
        return CaseMapper.ToDto(entity);
    }
}