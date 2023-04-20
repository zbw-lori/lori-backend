namespace lori.backend.Core.Models;
public class Robot
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string Description { get; set; } = null!;
  public string Model { get; set; } = null!;
  public bool IsAvailable { get; set; }
}
