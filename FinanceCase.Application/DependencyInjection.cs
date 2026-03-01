using FinanceCase.Application.Commands;
using FinanceCase.Application.Handlers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceCase.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<ICreateCaseHandler, CreateCaseHandler>();
        services.AddScoped<ISubmitCaseHandler, SubmitCaseHandler>();
        services.AddScoped<IMarkInReviewHandler, MarkInReviewHandler>();
        services.AddScoped<IApproveCaseHandler, ApproveCaseHandler>();
        services.AddScoped<IRejectCaseHandler, RejectCaseHandler>();

        return services;
    }
}