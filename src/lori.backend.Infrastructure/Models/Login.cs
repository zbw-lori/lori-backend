using System.ComponentModel.DataAnnotations;

namespace lori.backend.Infrastructure.Models;
public class Login
{
  [Key]
  public string Username { get; set; } = null!;
  public Byte[] Password { get; set; } = null!;
  public int RoleId { get; set; }
  public Role? Role { get; set; }
}
