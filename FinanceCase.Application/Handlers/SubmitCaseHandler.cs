using FinanceCase.Application.Abstractions;
using FinanceCase.Application.Commands;
using FinanceCase.Application.Dto;
using FinanceCase.Application.Mapping;
using FinanceCase.Domain.Exceptions;
using FinanceCase.Domain.Services;

namespace FinanceCase.Application.Handlers;

public class SubmitCaseHandler : ISubmitCaseHandler
{
    private readonly IFinanceCaseRepository _repo;
    private readonly IRiskCalculator _risk;

    public SubmitCaseHandler(IFinanceCaseRepository repo, IRiskCalculator risk)
    {
        _repo = repo;
        _risk = risk;
    }

    public async Task<CaseDto> Handle(SubmitCaseRequest request, CancellationToken ct)
    {
        var entity = await _repo.GetAsync(request.CaseId, ct)
            ?? throw new DomainException("Case not found.");

        entity.Submit(c => _risk.Calculate(c));

        await _repo.SaveChangesAsync(ct);
        return CaseMapper.ToDto(entity);
    }
}