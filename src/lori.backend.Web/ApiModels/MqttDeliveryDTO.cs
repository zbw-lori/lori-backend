namespace lori.backend.Web.ApiModels;

public class MqttDeliveryDTO
{
  public int OrderId { get; set; }
  public int Location { get; set; }
  public string Product { get; set; } = null!;
  public string DeliveryType { get; set; } = null!;
}
