using EasyNetQ;
using System;
using System.Threading;

namespace RabbitMqCore.RPC.EasyClient
{
    class Program
    {
        static IBus bus;
        static void Main(string[] args)
        {
            var bus = RabbitHutch.CreateBus("amqps://vrxiejxl:1VyAzgqMYgvVpOOz4ViA3EPKh7tD1S8G@barnacle.rmq.cloudamqp.com/vrxiejxl");

            bus.Advanced.Connected += Advanced_Connected;
            bus.Advanced.Disconnected += Advanced_Disconnected;


            Console.WriteLine("Preparing Request");
            for (int i = 2; i < 100; i++)
            {
                Thread.Sleep(int.Parse(args[1]));
                var response = bus.Rpc.Request<string, string>(args[0]);

                Console.WriteLine(response);
            }
          
            Console.Read();
        }

        private static void Advanced_Connected(object sender, EventArgs e)
        {
            var exchange = bus.Advanced.ExchangeDeclare("easy_net_q_rpc", "direct");
            var queue = bus.Advanced.QueueDeclare("test");
            bus.Advanced.Bind(exchange, queue, "test");
            Console.WriteLine("Bind Ok");
        }

        private static void Advanced_Disconnected(object sender, EventArgs e)
        {
            Console.WriteLine("{0} disconnected", DateTime.Now);
        }
    }
}
