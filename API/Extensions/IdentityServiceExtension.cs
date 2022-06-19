using System.Text;
using API.Models;
using API.Services;
using Infrastructure;
using Infrastructure.DataModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

internal static class IdentityServiceExtension
{
    internal static IServiceCollection AddIdentityServices
        (this IServiceCollection services, IConfiguration config)
    {
        services.AddDefaultIdentity<UserDataModel>(opt =>
            opt.SignIn.RequireConfirmedAccount = false
        ).AddEntityFrameworkStores<DataContext>();

        services.AddJwtService(config);

        return services;
    }

    // NOTE: JWT認証の実装方法について
    // https://book2525stack.com/2020/08/31/aspnetcore-jwt/
    // https://zenn.dev/mosuma/articles/ebfb55b9b7d629

    private static IServiceCollection AddJwtService
        (this IServiceCollection services, IConfiguration config)
    {
        var jwtSettings = new JwtSettings();
        var section = config.GetSection(nameof(JwtSettings));
        section.Bind(jwtSettings);
        services.Configure<JwtSettings>(section);
        services.AddSingleton(opt =>
            opt.GetRequiredService<IOptions<JwtSettings>>().Value
        );

        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Audience = jwtSettings.SiteUrl;
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new()
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
