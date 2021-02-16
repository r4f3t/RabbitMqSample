using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMqCore.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://vrxiejxl:1VyAzgqMYgvVpOOz4ViA3EPKh7tD1S8G@barnacle.rmq.cloudamqp.com/vrxiejxl");

            using (var connection = factory.CreateConnection())
            {
                Console.WriteLine("Connection Created on " + factory.Uri.AbsolutePath);
                using (var channel = connection.CreateModel())
                {
                    Console.WriteLine("Channel Created");
                    channel.QueueDeclare("testQueue", false, false, false, null);
                    Console.WriteLine("Queue Declared");
                    string message = "Hello World";

                    var bodyByte = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", routingKey: "testQueue", basicProperties: null, body: bodyByte);
                    Console.WriteLine("Message Sent...");


                }
            }

            Console.ReadLine();
        }
    }
}
