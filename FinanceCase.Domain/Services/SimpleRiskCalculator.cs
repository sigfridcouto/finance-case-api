using FinanceCase.Domain.Entities;

namespace FinanceCase.Domain.Services;

// Simple, deterministic scoring (demo) – no ML, no magic.
public class SimpleRiskCalculator : IRiskCalculator
{
    public int Calculate(FinanceCase.Domain.Entities.FinanceCase c)
    {
        // Example heuristic:
        // - Higher requested amount vs income => higher risk
        // - Longer term => slightly higher risk
        var ratio = (double)(c.RequestedAmount / Math.Max(1m, c.AnnualIncome));
        var score =
            (int)Math.Round(Math.Clamp(ratio * 60.0, 0, 80)) +
            (int)Math.Round(Math.Clamp((c.TermMonths - 12) / 24.0, 0, 20));

        return Math.Clamp(score, 0, 100);
    }
}