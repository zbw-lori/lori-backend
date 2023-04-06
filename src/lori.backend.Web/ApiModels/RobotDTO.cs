namespace lori.backend.Web.ApiModels;

public class RobotDTO
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string Description { get; set; } = null!;
  public string Model { get; set; } = null!;
}
