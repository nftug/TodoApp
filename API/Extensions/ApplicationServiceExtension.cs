using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure;

namespace API.Extensions;

internal static class ApplicationServiceExtension
{
    internal static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            // opt.UseInMemoryDatabase("TodoList");
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyMethod()
                    .AllowAnyHeader()
                    // .AllowCredentials()
                    .WithOrigins("http://localhost:3000");
            });
        });

        services.AddMediatR(typeof(Application.Todos.UseCases.List.Query).Assembly);

        var assemblies = System.Reflection.Assembly.GetExecutingAssembly()
            .CollectReferencedAssemblies(new[] { "Domain", "Infrastructure" });

        services.AddAssemblyTypes(assemblies, ServiceLifetime.Transient, "Repository");
        services.AddAssemblyTypes(assemblies, ServiceLifetime.Transient, "Service");

        return services;
    }
}