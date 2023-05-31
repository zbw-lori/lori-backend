using MQTTnet.Client;

namespace lori.backend.Core.Interfaces;
public interface IMqttService
{
  Task Connect();
  Task Disconnect();
  Task Publish(string topic, string data);
  Task Subscribe(string topic, Func<MqttApplicationMessageReceivedEventArgs, Task> callback);
  Task Unsubscribe(string topic);
}
