using FinanceCase.Application;
using FinanceCase.Domain.Exceptions;
using FinanceCase.Domain.Services;
using FinanceCase.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(o =>
{
    // keep defaults
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Clean architecture DI
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Domain service
builder.Services.AddSingleton<IRiskCalculator, SimpleRiskCalculator>();

// ProblemDetails (clean API errors)
builder.Services.AddProblemDetails(opt =>
{
    opt.Map<DomainException>(ex => new ProblemDetails
    {
        Title = "Business rule violation",
        Detail = ex.Message,
        Status = StatusCodes.Status400BadRequest
    });

    opt.Map<ValidationException>(ex => new ProblemDetails
    {
        Title = "Validation error",
        Detail = ex.Message,
        Status = StatusCodes.Status400BadRequest
    });
});

builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

app.UseProblemDetails();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();