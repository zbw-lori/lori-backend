namespace lori.backend.Web.ApiModels;

public class MqttDeliveryDTO
{
  public int OrderId { get; set; }
  public string Product { get; set; }
  public string DeliveryType { get; set; }
}
