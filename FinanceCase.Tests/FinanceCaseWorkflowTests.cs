using FinanceCase.Domain.Entities;
using FinanceCase.Domain.Enums;
using FinanceCase.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinanceCase.Tests;

[TestClass]
public class FinanceCaseWorkflowTests
{
    [TestMethod]
    public void Draft_CanSubmit_ThenInReview_ThenApprove_WhenRiskOk()
    {
        var customerId = Guid.NewGuid();
        var c = new FinanceCase.Domain.Entities.FinanceCase(customerId, 10_000m, 24, 60_000m);

        c.Submit(_ => 10);
        Assert.AreEqual(CaseStatus.Submitted, c.Status);

        c.MarkInReview();
        Assert.AreEqual(CaseStatus.InReview, c.Status);

        c.Approve(maxAllowedRiskScore: 90);
        Assert.AreEqual(CaseStatus.Approved, c.Status);

        Assert.IsTrue(c.Notes.Any(n => n.Message.Contains("approved", StringComparison.OrdinalIgnoreCase)));
    }

    [TestMethod]
    public void CannotApprove_WhenRiskTooHigh()
    {
        var customerId = Guid.NewGuid();
        var c = new FinanceCase.Domain.Entities.FinanceCase(customerId, 200_000m, 120, 20_000m);

        c.Submit(_ => 95);
        c.MarkInReview();

        var ex = Assert.ThrowsException<DomainException>(() => c.Approve(maxAllowedRiskScore: 65));
        StringAssert.Contains(ex.Message, "exceeds");
        Assert.AreEqual(CaseStatus.InReview, c.Status);
    }

    [TestMethod]
    public void Draft_CanBeUpdated_ButNotAfterSubmit()
    {
        var customerId = Guid.NewGuid();
        var c = new FinanceCase.Domain.Entities.FinanceCase(customerId, 10_000m, 24, 60_000m);

        c.UpdateDraft(12_000m, 36, 70_000m);
        Assert.AreEqual(12_000m, c.RequestedAmount);
        Assert.AreEqual(36, c.TermMonths);
        Assert.AreEqual(70_000m, c.AnnualIncome);

        c.Submit(_ => 10);

        Assert.ThrowsException<DomainException>(() => c.UpdateDraft(15_000m, 48, 80_000m));
    }

    [TestMethod]
    public void Reject_RequiresReason()
    {
        var customerId = Guid.NewGuid();
        var c = new FinanceCase.Domain.Entities.FinanceCase(customerId, 10_000m, 24, 60_000m);

        c.Submit(_ => 10);
        c.MarkInReview();

        Assert.ThrowsException<DomainException>(() => c.Reject("  "));
        Assert.AreEqual(CaseStatus.InReview, c.Status);
    }
}