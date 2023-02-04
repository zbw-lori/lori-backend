using Microsoft.AspNetCore.Mvc;
using Sample.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MqttClient _client;
        public MessageController()
        {
            _client = new MqttClient();
        }

        ~MessageController()
        {
            _client.Disconnect();
        }

        // GET: api/<MessageController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MessageController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MessageController>
        [HttpPost]
        public async Task Post([FromBody] string value)
        {
            Console.WriteLine($"Received message: {value}");
            Console.WriteLine("Publish message with topic 'test' on mqtt...");

            await _client.Connect();
            await _client.Publish("test", value);
            await _client.Subscribe("test/response");
            Console.WriteLine("Done");
        }

        // PUT api/<MessageController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MessageController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
