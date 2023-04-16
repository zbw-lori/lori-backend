using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using lori.backend.Core.Interfaces;
using lori.backend.Infrastructure.Data;
using lori.backend.Infrastructure.Models;
using lori.backend.Web.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace lori.backend.Web.Api.v1;

public class AuthController : BaseApiController
{
  private readonly LoriDbContext _context;
  private readonly IConfiguration _configuration;
  private readonly ILoginService _loginService;

  public AuthController(LoriDbContext context,
    IConfiguration configuration,
    ILoginService loginService)
  {
    _context = context;
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
  public async Task<ActionResult<Login>> Register(LoginDto request)
  {
    string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

    var login = new Login
    {
      Username = request.Username,
      PasswordHash = passwordHash,
      RoleId = 2 //set a default role!
    };

    _context.Logins.Add(login);
    await _context.SaveChangesAsync();

    return Ok(login);
  }

  [HttpPost("login")]
  public async Task<ActionResult<Login>> Login(LoginDto request)
  {
    var login = await _context.Logins.FindAsync(request.Username);
    if (login == null)
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
