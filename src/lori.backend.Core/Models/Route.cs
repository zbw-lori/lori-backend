using System.ComponentModel.DataAnnotations.Schema;

namespace lori.backend.Core.Models;
public class Route
{
  public int Id { get; set; }
  [Column(TypeName = "json")]
  public string PriorityList { get; set; } = null!;

  public int RobotId { get; set; }
  public Robot Robot { get; set; } = null!;

  public int OrderId { get; set; }
  public Order Order { get; set; } = null!;
}
