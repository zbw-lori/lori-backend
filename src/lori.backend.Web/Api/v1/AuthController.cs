using lori.backend.Core.Interfaces;
using lori.backend.Infrastructure.Data;
using lori.backend.Infrastructure.Models;
using lori.backend.Web.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lori.backend.Web.Api.v1;

public class AuthController : BaseApiController
{
  private readonly LoriDbContext _context;
  private readonly IConfiguration _configuration;
  private readonly ILoginService _loginService;
  private readonly ITokenService<Login> _tokenService;

  public AuthController(LoriDbContext context,
    IConfiguration configuration,
    ILoginService loginService,
    ITokenService<Login> tokenService)
  {
    _context = context;
    _configuration = configuration;
    _loginService = loginService;
    _tokenService = tokenService;
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

    string token = _tokenService.CreateToken(login);

    return Ok(token);
  }
}
