using System.ComponentModel.DataAnnotations;

namespace lori.backend.Core.Models;
public class Login
{
  [Key]
  public string Username { get; set; } = null!;
  public Byte[] Password { get; set; } = null!;
  public bool IsActive { get; set; }
  public int RoleId { get; set; }
  public Role? Role { get; set; }
}
