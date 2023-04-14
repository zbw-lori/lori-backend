using System.ComponentModel.DataAnnotations;

namespace lori.backend.Infrastructure.Models;
public class Role
{
  [Key]
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string Description { get; set; } = null!;
}
