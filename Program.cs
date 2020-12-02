using System;

namespace UDPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";
            Console.WriteLine("Server started");
            var server = new Server();

            while (true)
            {
                //var clientInput = Console.ReadLine();
                server.ReceiveResponse();
            }

            Console.ReadKey();
        }
    }
}
