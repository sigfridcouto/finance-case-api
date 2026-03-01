using FinanceCase.Application.Commands;
using FluentValidation;

namespace FinanceCase.Application.Validation;

public class CreateCaseValidator : AbstractValidator<CreateCaseRequest>
{
    public CreateCaseValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.RequestedAmount).GreaterThan(0);
        RuleFor(x => x.TermMonths).InclusiveBetween(6, 480);
        RuleFor(x => x.AnnualIncome).GreaterThan(0);
    }
}