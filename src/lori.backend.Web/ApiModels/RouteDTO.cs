namespace lori.backend.Web.ApiModels;

public class RouteDTO
{
  public int Id { get; set; }
  public string PriorityList { get; set; } = null!;
  public int RobotId { get; set; }
  public int OrderId { get; set; }
}
