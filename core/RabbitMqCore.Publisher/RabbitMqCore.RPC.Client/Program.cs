using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMqCore.RPC.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://vrxiejxl:1VyAzgqMYgvVpOOz4ViA3EPKh7tD1S8G@barnacle.rmq.cloudamqp.com/vrxiejxl");

            using (var connection = factory.CreateConnection())
            {
                using (var channel=connection.CreateModel())
                {
                    var props = channel.CreateBasicProperties();
                    props.ReplyTo = "responseQueue1";

                    var message = "2";

                    var messageBytes = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                                         routingKey: "rpc_queue",
                                         basicProperties: props,
                                         body: messageBytes);

                }
            }
            Console.ReadLine();
        }
    }
}
