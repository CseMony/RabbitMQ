using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IBus _busService;
        public UserController(IBus busService)
        {
            _busService = busService;
        }
        [HttpPost]
        public async Task<string> CreateUser(User user)
        {
            if (user != null)
            {
                user.Datetime = DateTime.Now;
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
               
                {
                    
                    channel.QueueDeclare(queue: "UserQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    //var queueName = channel.QueueDeclare().QueueName;
                    var json = JsonConvert.SerializeObject(user);
                    var body = Encoding.UTF8.GetBytes(json);
                  


                    
                    //channel.BasicPublish(exchange: "",
                    //             routingKey: queueName,
                    //             basicProperties: null,
                    //             body: body);
                    channel.BasicPublish(exchange: "ABC", routingKey: "UserQueue", basicProperties: null, body: body);

                    
                    return "true";
                }
            }
            return "false";
        }
    }
}
