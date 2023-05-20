using System.Text.Json;
using lori.backend.Core.Interfaces;
using lori.backend.Core.Models;
using lori.backend.Infrastructure.Data;

namespace lori.backend.Infrastructure.Services;
public class MqttRegistrationService : IMqttRegistrationService
{
  private readonly IMqttService _mqttService;
  private readonly LoriDbContext _context;

  public MqttRegistrationService(IMqttService mqttService, LoriDbContext context)
  {
    _mqttService = mqttService;
    _context = context;
  }

  public async Task Start()
  {
    await _mqttService.Connect();
    await _mqttService.Subscribe("register", async callback =>
    {
      var request = JsonSerializer.Deserialize<RegistrationRequest>(callback.ApplicationMessage.Payload);

      if (request != null && request.ClientId != null)
      {
        Console.WriteLine($"Register Client: {request.ClientId}");

        Robot? robot;
        robot = _context.Robots.SingleOrDefault(r => r.Model.Equals(request.ClientId));
        if (robot == null)
        {
          robot = new Robot
          {
            Name = "Test",
            Description = "Test",
            Model = request.ClientId,
            IsAvailable = true
          };

          _context.Robots.Add(robot);
          await _context.SaveChangesAsync();
        }

        var response = new RegistrationResponse() { RobotId = robot.Id };
        var data = JsonSerializer.Serialize<RegistrationResponse>(response);
        await _mqttService.Publish($"confirm/{request.ClientId}", data);
      }
    });
  }
}

class RegistrationRequest
{
  public string ClientId { get; set; } = null!;
}

class RegistrationResponse
{
  public int RobotId { get; set; }
}
