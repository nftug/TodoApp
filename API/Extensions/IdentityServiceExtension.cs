using System.Text;
using Infrastructure;
using Infrastructure.DataModels;
using Infrastructure.Shared.Services.AuthToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

internal static class IdentityServiceExtension
{
    internal static IServiceCollection AddIdentityServices
        (this IServiceCollection services, IConfiguration config)
    {
        services
            .AddDefaultIdentity<UserDataModel>(opt =>
                opt.SignIn.RequireConfirmedAccount = false
            )
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<DataContext>();

        services.AddAuthTokenService(config);

        return services;
    }

    // NOTE: JWT認証の実装方法について
    // https://book2525stack.com/2020/08/31/aspnetcore-jwt/
    // https://zenn.dev/mosuma/articles/ebfb55b9b7d629

    private static IServiceCollection AddAuthTokenService
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

        services.AddScoped<AuthTokenService>();

        return services;
    }
}
