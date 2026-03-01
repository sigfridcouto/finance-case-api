using FinanceCase.Application.Abstractions;
using FinanceCase.Application.Commands;
using FinanceCase.Application.Dto;
using FinanceCase.Application.Mapping;
using FinanceCase.Domain.Entities;

namespace FinanceCase.Application.Handlers;

public class CreateCaseHandler : ICreateCaseHandler
{
    private readonly IFinanceCaseRepository _repo;

    public CreateCaseHandler(IFinanceCaseRepository repo)
    {
        _repo = repo;
    }

    public async Task<CaseDto> Handle(CreateCaseRequest request, CancellationToken ct)
    {
        var entity = new FinanceCase.Domain.Entities.FinanceCase(
            request.CustomerId,
            request.RequestedAmount,
            request.TermMonths,
            request.AnnualIncome);

        await _repo.AddAsync(entity, ct);
        await _repo.SaveChangesAsync(ct);

        return CaseMapper.ToDto(entity);
    }
}