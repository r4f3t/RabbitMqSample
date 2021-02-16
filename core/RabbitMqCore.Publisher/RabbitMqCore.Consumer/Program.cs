using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMqCore.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://vrxiejxl:1VyAzgqMYgvVpOOz4ViA3EPKh7tD1S8G@barnacle.rmq.cloudamqp.com/vrxiejxl");

            var connection = factory.CreateConnection();
            {
                //Console.WriteLine("Connection Created on " + factory.Uri.AbsolutePath);
                using (var channel = connection.CreateModel())
                {
                    Console.WriteLine("Channel Created");
                    channel.QueueDeclare("testQueue", false, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);

                    channel.BasicConsume("testQueue", false, consumer);

                    consumer.Received += (model, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body.Span);

                        Console.WriteLine("Okunan Değer : " + message);
                    };


                }
            }

            Console.ReadLine();
        }
    }
}
