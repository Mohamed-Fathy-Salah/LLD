using Microsoft.Extensions.DependencyInjection;
using InstallmentPlanner.UnitOfWork;
using InstallmentPlanner.Services;
using InstallmentPlanner.Strategies;
using InstallmentPlanner.Factories;

namespace InstallmentPlanner.Configuration;

public static class ServiceConfiguration
{
    public static IServiceCollection AddInstallmentPlannerServices(this IServiceCollection services)
    {
        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // Register strategies
        services.AddScoped<InitiationAction>();
        services.AddScoped<CorridorAction>();
        services.AddScoped<EuriborAction>();
        services.AddScoped<SofrAction>();
        services.AddScoped<TerminationAction>();
        services.AddScoped<OffloadingAction>();
        services.AddScoped<SecuritizationAction>();

        // Register factory
        services.AddScoped<ActionFactory>();

        // Register services
        services.AddScoped<ICalculationService, CalculationService>();

        return services;
    }
}
