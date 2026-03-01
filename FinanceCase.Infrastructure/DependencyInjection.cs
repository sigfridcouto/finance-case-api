using FinanceCase.Application.Abstractions;
using FinanceCase.Infrastructure.Persistence;
using FinanceCase.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FinanceCase.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var cs = config.GetConnectionString("SqlServer")
                 ?? throw new InvalidOperationException("Connection string 'SqlServer' not found.");

        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(cs));

        services.AddScoped<IFinanceCaseRepository, FinanceCaseRepository>();

        return services;
    }
}