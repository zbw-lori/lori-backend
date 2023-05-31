using System.Text.Json;
using lori.backend.Core.Interfaces;
using lori.backend.Infrastructure.Services;

namespace lori.backend.RobotMock;

public class Program
{
  private static readonly string CLIENT_ID = "lori.robotmock";
  public static async Task Main(string[] args)
  {
    var mqttService = new MqttService(CLIENT_ID, "localhost", 1883);
    await mqttService.Connect();

    await mqttService.Subscribe($"confirm/{CLIENT_ID}", async callback =>
    {
      var response = JsonSerializer.Deserialize<RegistrationResponse>(callback.ApplicationMessage.Payload);
      if (response != null)
      {
        await subscribeWithRobotId(mqttService, response.RobotId);
      }
    });

    var registration = new RegistrationRequest() { ClientId = CLIENT_ID };
    var request = JsonSerializer.Serialize<RegistrationRequest>(registration);
    await mqttService.Publish("register", request);

    Console.WriteLine("Type Enter to exit...");
    var input = Console.ReadLine();

    Console.WriteLine("Disconnecting...");
    await mqttService.Disconnect();
    Console.WriteLine("Done");
  }

  private static async Task subscribeWithRobotId(IMqttService mqttService, int robotId)
  {
    await mqttService.Subscribe($"lori/{robotId}/order", callback =>
    {
      Console.WriteLine("Order request called!");
      return Task.CompletedTask;
    });

    await mqttService.Subscribe($"lori/{robotId}/delivery", callback =>
    {
      Console.WriteLine("Delivery request called!");
      return Task.CompletedTask;
    });

    await mqttService.Subscribe($"lori/{robotId}/confirm", callback =>
    {
      Console.WriteLine("Confirm order request called!");
      return Task.CompletedTask;
    });
  }
}