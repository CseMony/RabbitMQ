using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbit.Consumer
{
    public class Program
    {
        static void Main(string[] args)
        {


            Task.Run(() => Test());
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }


        public static void Test()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //channel.ExchangeDeclare(exchange: "direct_logs",
                //                    type: "direct");
                //var queueName = channel.QueueDeclare().QueueName;


                //foreach (var key in routing_keys)
                //{
                //    channel.QueueBind(queue: queueName,
                //                      exchange: "direct_logs",
                //                      routingKey: key);
                //}
                //channel.QueueBind(queue: "UserQueue",
                //                      exchange: "",
                //                      routingKey: "UserQueue");
                Console.WriteLine(" [*] Waiting for messages.");


                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    User user = JsonSerializer.Deserialize<User>(message);
                    var routingKey = ea.RoutingKey;
                    Thread.Sleep(5000);
                    Console.WriteLine(" [x] {0}:{1} ", message, routingKey);

                };
                channel.BasicConsume(queue: "UserQueue",
                                     autoAck: true,
                                     consumer: consumer);
            }


        }
    }
}
