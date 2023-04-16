using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using lori.backend.Core.Interfaces;
using lori.backend.Infrastructure.Models;
using lori.backend.Web.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace lori.backend.Web.Api.v1;

public class AuthController : BaseApiController
{
  public static Login login = new();
  private readonly IConfiguration _configuration;
  private readonly ILoginService _loginService;

  public AuthController(IConfiguration configuration, ILoginService loginService)
  {
    _configuration = configuration;
    _loginService = loginService;
  }

  [HttpGet, Authorize]
  public ActionResult<string> GetMe()
  {
    var userName = _loginService.GetUser();
    return Ok(userName);
  }

  [HttpPost("register")]
  public ActionResult<Login> Register(LoginDto request)
  {
    string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

    login.Username = request.Username;
    login.PasswordHash = passwordHash;

    return Ok(login);
  }

  [HttpPost("login")]
  public ActionResult<Login> Login(LoginDto request)
  {
    if (login.Username != request.Username)
    {
      return BadRequest("User not found");
    }

    if (!BCrypt.Net.BCrypt.Verify(request.Password, login.PasswordHash))
    {
      return BadRequest("Wrong password.");
    }

    string token = CreateToken(login);

    return Ok(token);
  }

  private string CreateToken(Login login)
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
