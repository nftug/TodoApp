using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Infrastructure;
using Domain.Comments.Entities;
using Infrastructure.Comments;
using Domain.Comments.Services;
using Domain.Todos.Entities;
using Infrastructure.Todos;
using Domain.Users.Entities;
using Infrastructure.Users;
using Domain.Todos.Services;
using Domain.Users.Services;
using Domain.Shared.Interfaces;
using Domain.Services;

namespace API.Extensions;

internal static class ApplicationServiceExtension
{
    internal static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration config)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
        });

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

        // repositories
        services.AddTransient<IRepository<Todo>, TodoRepository>();
        services.AddTransient<IRepository<Comment>, CommentRepository>();
        services.AddTransient<IRepository<User>, UserRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

        // Query services
        services.AddTransient<IFilterQueryService<Todo>, TodoFilterQueryService>();
        services.AddTransient<IFilterQueryService<Comment>, CommentFilterQueryService>();

        // domain services
        services.AddScoped<IDomainService<Todo>, TodoService>();
        services.AddScoped<IDomainService<Comment>, CommentService>();
        services.AddScoped<IDomainService<User>, UserService>();

        return services;
    }
}