using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.DataModels;

namespace Infrastructure.Shared.Services.AuthToken;

public class AuthTokenService
{
    private readonly JwtSettings _settings;

    public AuthTokenService(JwtSettings settings)
    {
        _settings = settings;
    }

    public string CreateToken(UserDataModel user)
    {
        var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(int.Parse(_settings.JwtExpireDay)),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public string SiteUrl { get; set; } = string.Empty;
    public string JwtExpireDay { get; set; } = string.Empty;
}