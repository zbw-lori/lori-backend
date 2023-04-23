using MQTTnet.Client;

namespace lori.backend.Core.Interfaces;
public interface IMqttService
{
  void Connect();
  void Disconnect();
  void Publish(string topic, string data);
  void Subscribe(string topic, Func<MqttApplicationMessageReceivedEventArgs, Task> callback);
  void Unsubscribe(string topic);
}
