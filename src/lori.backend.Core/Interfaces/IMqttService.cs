namespace lori.backend.Core.Interfaces;
public interface IMqttService
{
  void Connect();
  void Disconnect();
  void Publish(string topic, string data);
  void Subscribe(string topic, string data);
  void Unsubscribe(string topic, string data);
}
