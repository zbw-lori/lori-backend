using System.Text;
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
      var clientId = Encoding.Default.GetString(callback.ApplicationMessage.Payload);
      Console.WriteLine($"Register Client: {clientId}");

      var robot = new Robot
      {
        Name = "Test",
        Description = "Test",
        Model = clientId,
        IsAvailable = true
      };
      _context.Robots.Add(robot);
      await _context.SaveChangesAsync();

      await _mqttService.Publish($"confirm/{clientId}", $"RobotId: {robot.Id}");
    });
  }
}