using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMqCore.Publisher
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

        public static int ciriticalCount = 0, warningCount = 0, errorCount = 0, infoCount = 0;
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://vrxiejxl:1VyAzgqMYgvVpOOz4ViA3EPKh7tD1S8G@barnacle.rmq.cloudamqp.com/vrxiejxl");

            using (var connection = factory.CreateConnection())
            {
                Console.WriteLine("Connection Created on " + factory.Uri.AbsolutePath);
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("direct-exchange-logs", ExchangeType.Direct, durable: true);


                    var errorTypeArray = Enum.GetValues(typeof(ErrorType));



                    for (int i = 0; i < int.Parse(GetMessageCount(args)); i++)
                    {
                        var random = new Random();

                        var log = (ErrorType)errorTypeArray.GetValue(random.Next(errorTypeArray.Length));

                        string message = i+" - ErrMsg " + log.ToString();
                        var body = Encoding.UTF8.GetBytes(message);

                        var properties = channel.CreateBasicProperties();

                        properties.Persistent = true;

                        channel.BasicPublish(exchange: "direct-exchange-logs",
                                             routingKey: log.ToString(),
                                             basicProperties: properties,
                                             body: body);

                        Console.WriteLine(" [" + i + "] Sent {0}", message);
                        CalcSumOfEnum(log);
                    }

                    Console.WriteLine($"Critical:{ciriticalCount}      Warning:{warningCount}     Info:{infoCount}     Error:{errorCount}");

                }
            }

            Console.ReadLine();
        }
        private static void CalcSumOfEnum(ErrorType errorType)
        {

            switch (errorType)
            {
                case ErrorType.Critical:
                    ciriticalCount++;
                    break;
                case ErrorType.Warning:
                    warningCount++;
                    break;
                case ErrorType.Info:
                    infoCount++;
                    break;
                case ErrorType.Error:
                    errorCount++;
                    break;
                default:
                    break;
            }
        }

        private static string GetMessageCount(string[] args)
        {
            return args[0];
        }
    }
}
