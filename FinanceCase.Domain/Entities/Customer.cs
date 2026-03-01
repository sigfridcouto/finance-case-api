namespace FinanceCase.Domain.Entities;

public class Customer {
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string FullName { get; private set; } = string.Empty;
    public string? Email { get; private set; }

    private Customer() { } // EF

    public Customer(string fullName, string? email) {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Customer name is required.", nameof(fullName));

        FullName = fullName.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
    }
}