using System.Text.Json;
using lori.backend.Core.Interfaces;
using MQTTnet;
using MQTTnet.Client;

namespace lori.backend.Infrastructure.Services;
public class MqttService : IMqttService
{
  private readonly MqttFactory _factory;
  private readonly IMqttClient _client;
  private readonly string _clientId;
  private readonly string _server;
  private readonly int _port;

  public MqttService(string clientId = "backend", string server = "localhost", int port = 1803)
  {
    _factory = new MqttFactory();
    _client = _factory.CreateMqttClient();
    _clientId = clientId;
    _server = server;
    _port = port;
  }

  public async Task Connect()
  {
    if (_client.IsConnected)
    {
      return;
    }

    var options = new MqttClientOptionsBuilder()
                    .WithClientId(_clientId)
                    .WithTcpServer(_server, _port)
                    .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)
                    .Build();

    var response = await _client.ConnectAsync(options, CancellationToken.None);
    DumpToConsole(response);
  }

  public async Task Disconnect()
  {
    await _client.DisconnectAsync(MqttClientDisconnectReason.ImplementationSpecificError);
  }

  public async Task Publish(string topic, string data)
  {
    var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(data)
                    .Build();

    var response = await _client.PublishAsync(applicationMessage, CancellationToken.None);
    DumpToConsole(response);
  }

  public async Task Subscribe(string topic, Func<MqttApplicationMessageReceivedEventArgs, Task> callback)
  {
    var subscribeOptions = _factory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(f => f.WithTopic($"{topic}"))
                    .Build();

    var response = await _client.SubscribeAsync(subscribeOptions, CancellationToken.None);
    _client.ApplicationMessageReceivedAsync += callback;
    DumpToConsole(response);
  }

  public async Task Unsubscribe(string topic)
  {
    var unSubscribeOptions = _factory.CreateUnsubscribeOptionsBuilder()
                    .WithTopicFilter(topic)
                    .Build();
    var response = await _client.UnsubscribeAsync(unSubscribeOptions, CancellationToken.None);
    DumpToConsole(response);
  }

  private static TObject DumpToConsole<TObject>(TObject @object)
  {
    var output = "NULL";
    if (@object != null)
    {
      output = JsonSerializer.Serialize(@object, new JsonSerializerOptions
      {
        WriteIndented = true
      });
    }

    Console.WriteLine($"[{@object?.GetType().Name}]:\r\n{output}");
    return @object;
  }
}
