using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Infrastructure;
using Domain.Todo;
using Domain.Comment;
using Infrastructure.Todo;
using Infrastructure.Comment;
using Infrastructure.User;
using Domain.Interfaces;
using Domain.User;

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
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
            });
        });

        services.AddMediatR(typeof(Application.Todo.List.Query).Assembly);

        // repositories
        services.AddTransient<IRepository<TodoModel>, TodoRepository>();
        services.AddTransient<IRepository<CommentModel>, CommentRepository>();
        services.AddTransient<IRepository<UserModel>, UserRepository>();

        // query services
        services.AddTransient<IQueryService<TodoModel>, TodoQueryService>();
        services.AddTransient<IQueryService<CommentModel>, CommentQueryService>();

        // domain services
        services.AddScoped<IDomainService<TodoModel>, TodoService>();
        services.AddScoped<IDomainService<CommentModel>, CommentService>();
        services.AddScoped<IDomainService<UserModel>, UserService>();

        // AutoMapper
        services.AddAutoMapper(typeof(TodoRepositoryMapping).Assembly);
        services.AddAutoMapper(typeof(CommentRepositoryMapping).Assembly);
        services.AddAutoMapper(typeof(UserRepositoryMapping).Assembly);

        return services;
    }
}