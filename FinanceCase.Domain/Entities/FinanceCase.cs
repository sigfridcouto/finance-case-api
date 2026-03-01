using FinanceCase.Domain.Enums;
using FinanceCase.Domain.Exceptions;

namespace FinanceCase.Domain.Entities;

public class FinanceCase
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid CustomerId { get; private set; }
    public Customer? Customer { get; private set; }

    public CaseStatus Status { get; private set; } = CaseStatus.Draft;

    // Domain fields (minimal but “real”)
    public decimal RequestedAmount { get; private set; }
    public int TermMonths { get; private set; } // e.g. 12..360
    public decimal AnnualIncome { get; private set; }

    // Simulated risk value computed at submit/review
    public int RiskScore { get; private set; } = 0; // 0..100

    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; private set; }

    private readonly List<CaseNote> _notes = new();
    public IReadOnlyCollection<CaseNote> Notes => _notes.AsReadOnly();

    private FinanceCase() { } // EF

    public FinanceCase(Guid customerId, decimal requestedAmount, int termMonths, decimal annualIncome)
    {
        if (customerId == Guid.Empty) throw new ArgumentException("CustomerId is required.");
        ValidateAmounts(requestedAmount, termMonths, annualIncome);

        CustomerId = customerId;
        RequestedAmount = requestedAmount;
        TermMonths = termMonths;
        AnnualIncome = annualIncome;

        AddNote("Case created (Draft).");
    }

    public void UpdateDraft(decimal requestedAmount, int termMonths, decimal annualIncome)
    {
        EnsureStatus(CaseStatus.Draft, "Only Draft cases can be edited.");

        ValidateAmounts(requestedAmount, termMonths, annualIncome);

        RequestedAmount = requestedAmount;
        TermMonths = termMonths;
        AnnualIncome = annualIncome;
        Touch();

        AddNote("Draft updated.");
    }

    public void Submit(Func<FinanceCase, int> riskCalculator)
    {
        EnsureStatus(CaseStatus.Draft, "Only Draft cases can be submitted.");

        // “Required data” rule (simple but meaningful)
        if (RequestedAmount <= 0 || TermMonths <= 0 || AnnualIncome <= 0)
            throw new DomainException("Cannot submit case: missing required data.");

        RiskScore = riskCalculator(this);
        Status = CaseStatus.Submitted;
        Touch();

        AddNote($"Case submitted. RiskScore={RiskScore}.");
    }

    public void MarkInReview()
    {
        EnsureStatus(CaseStatus.Submitted, "Only Submitted cases can enter review.");

        Status = CaseStatus.InReview;
        Touch();

        AddNote("Case moved to InReview.");
    }

    public void Approve(int maxAllowedRiskScore)
    {
        EnsureStatus(CaseStatus.InReview, "Only InReview cases can be approved.");

        if (RiskScore > maxAllowedRiskScore)
            throw new DomainException($"Cannot approve: RiskScore {RiskScore} exceeds allowed threshold {maxAllowedRiskScore}.");

        Status = CaseStatus.Approved;
        Touch();

        AddNote("Case approved.");
    }

    public void Reject(string reason)
    {
        EnsureStatus(CaseStatus.InReview, "Only InReview cases can be rejected.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("Reject reason is required.");

        Status = CaseStatus.Rejected;
        Touch();

        AddNote($"Case rejected. Reason: {reason.Trim()}");
    }

    private void ValidateAmounts(decimal requestedAmount, int termMonths, decimal annualIncome)
    {
        if (requestedAmount <= 0) throw new DomainException("RequestedAmount must be > 0.");
        if (termMonths < 6 || termMonths > 480) throw new DomainException("TermMonths must be between 6 and 480.");
        if (annualIncome <= 0) throw new DomainException("AnnualIncome must be > 0.");
    }

    private void EnsureStatus(CaseStatus expected, string messageIfNot)
    {
        if (Status != expected) throw new DomainException(messageIfNot);
    }

    private void AddNote(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;
        _notes.Add(new CaseNote(Id, message));
    }

    private void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
}