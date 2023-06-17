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

  [HttpPost("package/grab/{id}")]
  [SwaggerOperation(
  Summary = "Sends command to grab package of specific robot",
  OperationId = "Mqtt.GrabPackage")]
  public async Task<IActionResult> GrabPackage(int id, int package)
  {
    await _mqttService.Connect();
    await _mqttService.Publish($"lori/{id}/grabPackage", package.ToString());
    return NoContent();
  }

  [HttpPost("package/store/{id}")]
  [SwaggerOperation(
  Summary = "Sends command to store package of specific robot",
  OperationId = "Mqtt.StorePackage")]
  public async Task<IActionResult> StorePackage(int id, int package)
  {
    await _mqttService.Connect();
    await _mqttService.Publish($"lori/{id}/storePackage", package.ToString());
    return NoContent();
  }

  [HttpPost("package/deliver/{id}")]
  [SwaggerOperation(
  Summary = "Sends command to deliver package of specific robot",
  OperationId = "Mqtt.DeliverPackage")]
  public async Task<IActionResult> DeliverPackage(int id, int package)
  {
    await _mqttService.Connect();
    await _mqttService.Publish($"lori/{id}/deliverPackage", package.ToString());
    return NoContent();
  }

  [HttpPost("drive/store/{id}")]
  [SwaggerOperation(
  Summary = "Sends command to drive specific robot to store",
  OperationId = "Mqtt.DriveToStore")]
  public async Task<IActionResult> DriveToStore(int id)
  {
    await _mqttService.Connect();
    await _mqttService.Publish($"lori/{id}/driveToStore", "");
    return NoContent();
  }

  [HttpPost("drive/package/{id}")]
  [SwaggerOperation(
  Summary = "Sends command to drive specific robot to package",
  OperationId = "Mqtt.DriveToPackage")]
  public async Task<IActionResult> DriveToPackage(int id, int package)
  {
    await _mqttService.Connect();
    await _mqttService.Publish($"lori/{id}/driveToPackage", package.ToString());
    return NoContent();
  }

  [HttpPost("drive/customer/{id}")]
  [SwaggerOperation(
  Summary = "Sends command to drive specific robot to customer",
  OperationId = "Mqtt.DriveToCustomer")]
  public async Task<IActionResult> DriveToCustomer(int id, int customer)
  {
    await _mqttService.Connect();
    await _mqttService.Publish($"lori/{id}/driveToCustomer", customer.ToString());
    return NoContent();
  }

  [HttpPost("drive/start/{id}")]
  [SwaggerOperation(
  Summary = "Sends command to drive specific robot to start",
  OperationId = "Mqtt.DriveToStart")]
  public async Task<IActionResult> DriveToStart(int id)
  {
    await _mqttService.Connect();
    await _mqttService.Publish($"lori/{id}/driveToStart", "");
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
