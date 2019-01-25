using System;
using System.Text;
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
            //MQTTClientLib.MqttClient client = new MQTTClientLib.MqttClient();

            //client.Connect("localhost", 1883).Wait();
            ////client.Subscribe()
            //client.Publish("t", "xxx");

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
            MQTTClientLib.MqttClient client = new MQTTClientLib.MqttClient();

            client.Connect("localhost", 1883).Wait();
            //client.Subscribe()
            client.Publish("t", "xxx " + id.ToString());

            //var options = new MqttClientOptionsBuilder()
            //    .WithTcpServer("localhost", 1883)
            //    .Build();

            //_client = new MqttFactory().CreateMqttClient();

            //await _client.ConnectAsync(options);

            //var message = new MqttApplicationMessageBuilder()
            //    .WithTopic("t")
            //    .WithPayload("cc")
            //    .WithExactlyOnceQoS()
            //    .WithRetainFlag()
            //    .Build();

            //_client.PublishAsync(message).Wait();

            return Ok("xxx");


            //string mes = "| ";

            //var options = new MqttClientOptionsBuilder()
            //    .WithTcpServer("localhost", 1883)
            //    .Build();

            //_client = new MqttFactory().CreateMqttClient();

            //_client.Connected += async (s, e) =>
            //{
            //    await _client.SubscribeAsync(new TopicFilterBuilder().WithTopic("t").Build());
            //};

            //_client.ApplicationMessageReceived += (s, e) =>
            //{
            //    mes += Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            //};

            //await _client.ConnectAsync(options);

            //return Ok(mes);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] string value)
        {
            string mes = "emptyy";

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            _client = new MqttFactory().CreateMqttClient();

            _client.Connected += async (s, e) =>
            {
                await _client.SubscribeAsync(new TopicFilterBuilder().WithTopic("t").Build());
            };

            _client.ApplicationMessageReceived += (s, e) =>
            {
                mes += Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            };

            await _client.ConnectAsync(options);

            return Ok(mes);
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
