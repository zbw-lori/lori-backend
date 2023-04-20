using System.ComponentModel.DataAnnotations;

namespace lori.backend.Core.Models;
public class Login
{
  [Key]
  public string Username { get; set; } = null!;
  public string PasswordHash { get; set; } = null!;
  public bool IsActive { get; set; }
  public int RoleId { get; set; }
  public Role? Role { get; set; }
}
