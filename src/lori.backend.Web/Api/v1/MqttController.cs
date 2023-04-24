using System.Text.Json;
using lori.backend.Core.Interfaces;
using lori.backend.Web.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace lori.backend.Web.Api.v1;

public class MqttController : BaseApiController
{
  private readonly IMqttService _mqttService;

  public MqttController(IMqttService mqttService)
  {
    _mqttService = mqttService;
  }

  ~MqttController()
  {
    _mqttService.Disconnect();  //todo: don't call async method?
  }

  [HttpPost("order/{id}")]
  [SwaggerOperation(
  Summary = "Sends an order to the specific robot",
  OperationId = "Mqtt.PostOrder")]
  public async Task<IActionResult> PostOrder(int id, MqttOrderDTO order)
  {
    await _mqttService.Connect();
    var data = JsonSerializer.Serialize(order);
    await _mqttService.Publish($"lori/{id}/order", data);
    return NoContent();
  }

  [HttpPost("delivery/{id}")]
  [SwaggerOperation(
  Summary = "Sends delivery details to specific robot",
  OperationId = "Mqtt.PostDelivery")]
  public async Task<IActionResult> PostDelivery(int id, MqttDeliveryDTO delivery)
  {
    await _mqttService.Connect();
    var data = JsonSerializer.Serialize(delivery);
    await _mqttService.Publish($"lori/{id}/delivery", data);
    return NoContent();
  }

  [HttpPost("confirm/{id}")]
  [SwaggerOperation(
  Summary = "Sends confirmation of deliveryType to specific robot",
  OperationId = "Mqtt.ConfirmOrder")]
  public async Task<IActionResult> ConfirmOrder(int id, int orderId)
  {
    await _mqttService.Connect();
    var data = JsonSerializer.Serialize(orderId);
    await _mqttService.Publish($"lori/{id}/confirm", data);
    return NoContent();
  }
}
