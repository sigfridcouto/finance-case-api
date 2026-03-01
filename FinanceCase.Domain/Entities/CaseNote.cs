namespace FinanceCase.Domain.Entities;

public class CaseNote {
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid FinanceCaseId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

    public string Message { get; private set; } = string.Empty;

    private CaseNote() { } // EF

    internal CaseNote(Guid financeCaseId, string message) {
        FinanceCaseId = financeCaseId;
        Message = message.Trim();
    }
}