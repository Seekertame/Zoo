using Microsoft.Extensions.DependencyInjection;
using Zoo.Application.Interfaces;
using Zoo.Application.UseCases.Admission;
using Zoo.Application.UseCases.Inventory;
using Zoo.Application.UseCases.Reports;
using Zoo.Infrastructure.Clinics;
using Zoo.Infrastructure.Persistence.InMemory;
using Zoo.Infrastructure.Policies;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZooCore(this IServiceCollection services)
    {
        services.AddScoped<IAdmissionService, AdmissionService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IThingService, ThingService>();
        return services;
    }

    public static IServiceCollection AddZooInfra(this IServiceCollection services)
    {
        services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
        services.AddSingleton<IThingRepository, InMemoryThingRepository>();
        services.AddSingleton<IUnitOfWork, InMemoryUnitOfWork>();
        services.AddSingleton<IVeterinaryClinic, RandomizedVeterinaryClinic>();
        services.AddSingleton<IInventoryPolicy, InMemoryInventoryPolicy>();
        return services;
    }
}

