using System.Security.Claims;
using lori.backend.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace lori.backend.Infrastructure.Services;
public class LoginService : ILoginService
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public LoginService(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }
  public string GetUser()
  {
    var result = string.Empty;
    if (_httpContextAccessor.HttpContext != null)
    {
      result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
    }
    return result;
  }
}
