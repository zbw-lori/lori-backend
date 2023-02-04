using MQTTnet;
using MQTTnet.Client;

namespace Sample.Services
{
    public class MqttClient
    {
        private readonly MqttFactory _factory;
        private readonly IMqttClient _client;
        private readonly string _clientId = "backend";
        private readonly string _server = "localhost";
        private readonly int _port = 1883;
        public MqttClient()
        {
            _factory = new MqttFactory();
            _client = _factory.CreateMqttClient();
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
                    .Build();

            await _client.ConnectAsync(options, CancellationToken.None);
        }

        public void Disconnect()
        {
            //await _client.DisconnectAsync(); //todo: do we need to call disconnect?
            _client.Dispose();
        }

        public async Task Publish(string topic, string message)
        {
            var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(message)
                    .Build();

            await _client.PublishAsync(applicationMessage, CancellationToken.None);
        }

        public async Task Subscribe(string topic)
        {
            var subscribeOptions = _factory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(
                        f =>
                        {
                            f.WithTopic($"{topic}");
                        })
                    .Build();

            var response = await _client.SubscribeAsync(subscribeOptions, CancellationToken.None);
            _client.ApplicationMessageReceivedAsync += e =>
            {
                var message = System.Text.Encoding.Default.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine(message);
                return Task.CompletedTask;
            };

            Console.WriteLine("MQTT client subscribed to topic.");
        }
    }
}
