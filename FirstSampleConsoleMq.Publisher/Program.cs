using RabbitMQ.Client;
using System;
using System.Text;

namespace FirstSampleConsoleMq.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://vrxiejxl:1VyAzgqMYgvVpOOz4ViA3EPKh7tD1S8G@barnacle.rmq.cloudamqp.com/vrxiejxl");

            using (var connection=factory.CreateConnection())
            {
                Console.WriteLine("Connection Created on "+factory.Uri.AbsolutePath);
                using (var channel=connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

                    string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "hello",
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.ReadLine();
        }
    }
}
