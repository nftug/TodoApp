using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;
using Application.TodoItems;

namespace API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplications(this IServiceCollection services, IConfiguration? config)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            });
            services.AddDbContext<TodoContext>(opt =>
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
            services.AddControllers().AddNewtonsoftJson(settings =>
            {
                settings.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddMediatR(typeof(List.Handler).Assembly);
            return services;
        }
    }
}