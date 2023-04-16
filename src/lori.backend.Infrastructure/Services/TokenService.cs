using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using lori.backend.Core.Interfaces;
using lori.backend.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace lori.backend.Infrastructure.Services;
public class TokenService : ITokenService<Login>
{
  private readonly IConfiguration _configuration;

  public TokenService(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public string CreateToken(Login login)
  {
    var expiration = DateTime.UtcNow.AddMinutes(30);
    var token = CreateJwtToken(CreateClaims(login), CreateSigningCredentials(), expiration);

    var tokenHandler = new JwtSecurityTokenHandler();
    return tokenHandler.WriteToken(token);
  }

  private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration)
  {
    return new(
                _configuration.GetValue<string>("AppSettings:Issuer"),
                _configuration.GetValue<string>("AppSettings:Audience"),
                claims,
                expires: expiration,
                signingCredentials: credentials
            );
  }

  private List<Claim> CreateClaims(Login login)
  {
    try
    {
      var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, "TokenForTheApiWithAuth"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                    //new Claim(ClaimTypes.NameIdentifier, login.id),
                    new Claim(ClaimTypes.Name, login.Username),
                    //new Claim(ClaimTypes.Email, login.Email)
                };
      return claims;
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }

  private SigningCredentials CreateSigningCredentials()
  {
    return new SigningCredentials(
        new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!)
        ),
        SecurityAlgorithms.HmacSha256
    );
  }
}
