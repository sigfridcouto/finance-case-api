using FinanceCase.Api.Validation;
using FinanceCase.Application.Commands;
using FinanceCase.Application.Dto;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FinanceCase.Api.Controllers;

[ApiController]
[Route("cases")]
public class CasesController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CaseDto>> Create(
        [FromBody] CreateCaseRequest request,
        [FromServices] ICreateCaseHandler handler,
        [FromServices] IValidator<CreateCaseRequest> validator,
        CancellationToken ct)
    {
        var validation = await validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return BadRequest(ValidationProblemFactory.From(validation));

        var result = await handler.Handle(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CaseDto>> GetById(
        [FromRoute] Guid id,
        [FromServices] FinanceCase.Application.Abstractions.IFinanceCaseRepository repo,
        CancellationToken ct)
    {
        var entity = await repo.GetAsync(id, ct);
        if (entity is null) return NotFound();

        return Ok(FinanceCase.Application.Mapping.CaseMapper.ToDto(entity));
    }

    [HttpPost("{id:guid}/submit")]
    public async Task<ActionResult<CaseDto>> Submit(
        [FromRoute] Guid id,
        [FromServices] ISubmitCaseHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new SubmitCaseRequest(id), ct);
        return Ok(result);
    }

    [HttpPost("{id:guid}/review")]
    public async Task<ActionResult<CaseDto>> Review(
        [FromRoute] Guid id,
        [FromServices] IMarkInReviewHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new MarkInReviewRequest(id), ct);
        return Ok(result);
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<ActionResult<CaseDto>> Approve(
        [FromRoute] Guid id,
        [FromServices] IApproveCaseHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new ApproveCaseRequest(id), ct);
        return Ok(result);
    }

    public record RejectBody(string Reason);

    [HttpPost("{id:guid}/reject")]
    public async Task<ActionResult<CaseDto>> Reject(
        [FromRoute] Guid id,
        [FromBody] RejectBody body,
        [FromServices] IRejectCaseHandler handler,
        [FromServices] IValidator<RejectCaseRequest> validator,
        CancellationToken ct)
    {
        var req = new RejectCaseRequest(id, body.Reason);

        var validation = await validator.ValidateAsync(req, ct);
        if (!validation.IsValid)
            return BadRequest(ValidationProblemFactory.From(validation));

        var result = await handler.Handle(req, ct);
        return Ok(result);
    }

    private Dictionary<string, string[]> ValidationProblemDictionary(Dictionary<string, string[]> dict) => dict;
}