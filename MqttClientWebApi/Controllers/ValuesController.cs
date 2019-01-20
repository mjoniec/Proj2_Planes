using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace MqttClientWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        IMqttClient _client;

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            //var options = new ManagedMqttClientOptionsBuilder()
            //    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            //    .WithClientOptions(new MqttClientOptionsBuilder()
            //        .WithClientId("Client1")
            //        .WithTcpServer("localhost", 1883)
            //        .WithTls().Build())
            //    .Build();

            //_client = new MqttFactory().CreateManagedMqttClient();

            //await _client.StartAsync(options);



            return Ok("started");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            _client = new MqttFactory().CreateMqttClient();

            await _client.ConnectAsync(options);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("t")
                .WithPayload("cc")
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            _client.PublishAsync(message).Wait();

            return Ok("xxx");
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
