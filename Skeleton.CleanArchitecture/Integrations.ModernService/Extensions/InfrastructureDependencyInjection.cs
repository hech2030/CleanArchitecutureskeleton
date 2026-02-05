using Integrations.ModernService.Domain.Entities.Common.Options;
using Integrations.ModernService.Infrastructure.Persistence;
using Integrations.ModernService.Infrastructure.RabbitMq;
using Microsoft.EntityFrameworkCore;

namespace Integrations.ModernService.Extensions;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySQL(connectionString));
        services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMq"));
        services.AddHostedService<RabbitMqListener>();
        return services;
    }
}