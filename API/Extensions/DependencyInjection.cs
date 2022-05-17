using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Infrastructure;
using Application.Todos;
using Domain.Todos;
using Domain.Comments;
using Infrastructure.Todos;
using Infrastructure.Comments;
using Domain;
using API.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;

namespace API.Extensions;

internal static class DependencyInjection
{
    internal static IServiceCollection AddApplications(this IServiceCollection services, IConfiguration config)
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

        services.AddControllers().AddNewtonsoftJson(settings =>
        {
            settings.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });

        services.AddDefaultIdentity<ApplicationUser>(opt =>
            opt.SignIn.RequireConfirmedAccount = false
        ).AddEntityFrameworkStores<DataContext>();

        services.AddMediatR(typeof(List.Query).Assembly);

        services.AddJwtService(config);

        // repositories
        services.AddTransient<ITodoRepository, TodoRepository>();
        services.AddTransient<ICommentRepository, CommentRepository>();

        // query services
        services.AddTransient<TodoQuerySearchService>();
        services.AddTransient<CommentQuerySearchService>();

        return services;
    }

    // NOTE: JWT認証の実装方法について
    // https://book2525stack.com/2020/08/31/aspnetcore-jwt/
    // https://zenn.dev/mosuma/articles/ebfb55b9b7d629

    private static IServiceCollection AddJwtService(this IServiceCollection services, IConfiguration config)
    {
        var jwtSettings = new JwtSettings();
        var section = config.GetSection(nameof(JwtSettings));
        section.Bind(jwtSettings);
        services.Configure<JwtSettings>(section);
        services.AddSingleton<JwtSettings>(opt =>
            opt.GetRequiredService<IOptions<JwtSettings>>().Value
        );

        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Audience = jwtSettings.SiteUrl;
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateAudience = false,
                    };
                });

        services.AddScoped<TokenService>();

        return services;
    }
}