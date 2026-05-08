using ElectronicLibrary.Core.Interfaces;
using ElectronicLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicLibrary.Infrastructure;

/// <summary>
/// Дозволяє приховати деталі реєстрації сервісів бази даних від головного проєкту.
/// </summary>
// Refactoring Technique: Extract Method - винесено логіку реєстрації сервісів з майбутнього Program.cs в окремий метод.
// Principle: Separation of Concerns - шар відокремлює свою власну конфігурацію.
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}