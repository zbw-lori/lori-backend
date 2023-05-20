using System.Text.Json;
using lori.backend.Core.Interfaces;
using lori.backend.Infrastructure.Data;

namespace lori.backend.Infrastructure.Services;
public class MqttLiveDataService : IMqttLiveDataService
{
  private readonly IMqttService _mqttService;
  private readonly LoriDbContext _context;

  public MqttLiveDataService(IMqttService mqttService, LoriDbContext context)
  {
    _mqttService = mqttService;
    _context = context;
  }

  public async Task Start()
  {
    await _mqttService.Connect();
    await _mqttService.Subscribe("lori/+/livedata", async callback =>
    {
      var robotId = int.Parse(callback.ApplicationMessage.Topic.Split("/")[1]);
      Console.WriteLine($"Update live data of Robot: {robotId}");
      var data = JsonSerializer.Deserialize<LiveData>(callback.ApplicationMessage.Payload);
      var robot = await _context.Robots.FindAsync(robotId);
      if (data != null && robot != null)
      {
        robot.IsAvailable = data.IsAvailable;
        await _context.SaveChangesAsync();
      }
    });
  }
}

class LiveData
{
  public double Capacity { get; set; }
  public string Status { get; set; } = null!;
  public bool IsAvailable { get; set; }
}
