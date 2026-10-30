using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TuApp.Domain.Repositories;
using TuApp.Infrastructure.Adapters.Repositories;
using TuApp.Infrastructure.Context;

namespace TuApp.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "No se encontró la cadena de conexión DefaultConnection.");
        }

        services.AddDbContext<TicketeraContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped(
            typeof(IGenericRepository<>),
            typeof(GenericRepository<>));

        return services;
    }
}