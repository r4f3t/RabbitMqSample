using EasyNetQ;
using System;

namespace RabbitMqCore.Rpc.EasyServer
{
    class Program
    {
        static IBus bus;
        static void Main(string[] args)
        {
            var bus = RabbitHutch.CreateBus("amqps://vrxiejxl:1VyAzgqMYgvVpOOz4ViA3EPKh7tD1S8G@barnacle.rmq.cloudamqp.com/vrxiejxl");

            bus.Advanced.Connected += Advanced_Connected;
            bus.Advanced.Disconnected += Advanced_Disconnected;


            Console.WriteLine("Preparing Response");
            bus.Rpc.Respond<string, string>(request =>
            {
                Console.WriteLine(request+" Come from Client");
                return request + " Come From Server";
            });



            Console.ReadLine();

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

        private static string getFibString(string n)
        {
            Console.WriteLine("Respond For : " + n);
            return fib(int.Parse(n)).ToString();
        }

        private static int fib(int n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }

            return fib(n - 1) + fib(n - 2);
        }
    }
}
