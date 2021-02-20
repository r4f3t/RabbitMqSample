using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace RabbitMqCore.Consumer
{
    class Program
    {
        public enum ErrorType
        {
            Critical = 1,
            Warning = 2,
            Info = 3,
            Error = 4
        }

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://vrxiejxl:1VyAzgqMYgvVpOOz4ViA3EPKh7tD1S8G@barnacle.rmq.cloudamqp.com/vrxiejxl");

            var connection = factory.CreateConnection();
            {
                Console.WriteLine("Connection Created on " + factory.Uri.AbsolutePath);
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("direct-exchange-logs", ExchangeType.Direct, durable: true);

                    var queueName = channel.QueueDeclare().QueueName;

                    var routingKeys = GetRoutingKeys(args);

                    foreach (var item in routingKeys)
                    {
                        var expectingKey = ((ErrorType)int.Parse(item)).ToString();
                        Console.WriteLine("Instance Expecting "+expectingKey);
                        channel.QueueBind(queueName, "direct-exchange-logs",expectingKey);
                    }

                    var consumer = new EventingBasicConsumer(channel);

                    channel.BasicQos(0, 1, false);

                    channel.BasicConsume(queue: queueName,
                                       autoAck: false,
                                       consumer: consumer);

                    consumer.Received += (model, ea) =>
                    {
                        Thread.Sleep(int.Parse(GetSleepTime(args)));
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                        channel.BasicAck(ea.DeliveryTag, false);
                    };


                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }

            Console.ReadLine();
        }

        private static string GetSleepTime(string[] args)
        {
            return args[0];
        }

        private static string[] GetRoutingKeys(string[] args)
        {
            var tempArray = args.Skip(1);
            return tempArray.ToArray();
        }
    }
}
