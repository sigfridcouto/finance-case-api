using FinanceCase.Application.Commands;
using FluentValidation;

namespace FinanceCase.Application.Validation;

public class RejectCaseValidator : AbstractValidator<RejectCaseRequest>
{
    public RejectCaseValidator()
    {
        RuleFor(x => x.CaseId).NotEmpty();
        RuleFor(x => x.Reason).NotEmpty().MinimumLength(3).MaximumLength(500);
    }
}