using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WEBAPI.Application.Interfaces;
using WEBAPI.Infrastructure.Data;
using WEBAPI.Infrastructure.Services;

namespace WEBAPI.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // sql server kullanarak veritabaný baðlantýsý için DbContext'i kaydediyoruz. Connection string'i appsettings.json'dan alýyoruz.
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // TodoService'i ITodoService arayüzü ile eþleþtirerek dependency injection'a kaydediyoruz. Böylece controller'larda ITodoService kullanarak TodoService'in fonksiyonlarýný kullanabiliriz.
        services.AddScoped<ITodoService, TodoService>();

        return services;
    }
}
