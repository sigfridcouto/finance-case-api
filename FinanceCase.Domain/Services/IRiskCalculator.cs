using FinanceCase.Domain.Entities;

namespace FinanceCase.Domain.Services;

public interface IRiskCalculator
{
    int Calculate(FinanceCase.Domain.Entities.FinanceCase financeCase);
}