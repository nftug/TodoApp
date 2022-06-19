using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Infrastructure;
using Domain.Todos;
using Domain.Comments;
using Domain.Users;
using Infrastructure.DataModels;
using Infrastructure.Todos;
using Infrastructure.Comments;
using Infrastructure.Users;
using Domain.Interfaces;

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

        services.AddMediatR(typeof(Application.Todos.List.Query).Assembly);

        // repositories
        services.AddTransient<IRepository<Todo, TodoDataModel>, TodoRepository>();
        services.AddTransient<IRepository<Comment, CommentDataModel>, CommentRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

        // query services
        services.AddTransient<IQuerySearch<TodoDataModel>, TodoQuerySearchService>();
        services.AddTransient<IQuerySearch<CommentDataModel>, CommentQuerySearchService>();

        // AutoMapper
        services.AddAutoMapper(typeof(TodoRepositoryMapping).Assembly);
        services.AddAutoMapper(typeof(CommentRepositoryMapping).Assembly);

        return services;
    }
}