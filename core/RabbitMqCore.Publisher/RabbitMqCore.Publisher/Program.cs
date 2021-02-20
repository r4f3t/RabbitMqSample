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
                    channel.QueueDeclare(queue: "queueFirst",
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

                    for (int i = 0; i < int.Parse(GetMessageCount(args)); i++)
                    {

                        string message = "MEssage "+i;
                        var body = Encoding.UTF8.GetBytes(message);

                        var properties = channel.CreateBasicProperties();

                        properties.Persistent = true;

                        channel.BasicPublish(exchange: "",
                                             routingKey: "queueFirst",
                                             basicProperties: properties,
                                             body: body);
                        Console.WriteLine(" ["+i+"] Sent {0}", message);

                    }

                }
            }

            Console.ReadLine();
        }

        private static string GetMessageCount(string[] args)
        {
            return args[0];
        }
    }
}
