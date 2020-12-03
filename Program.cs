using System;

namespace UDPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";
            Console.WriteLine("Server started ... ");
            var server = new Server();

            string data1 = "data";

            bool isSecured = false;

            while (true)
            {

                while (!isSecured)
                {
                    server.ReceiveResponse();

                    server.SendRequest(data1);
                    isSecured = true;
                }

                server.ReceiveResponse();


                //var clientInput = Console.ReadLine();
                //server.SendRequest(clientInput);


            }

            Console.ReadKey();
        }
    }
}
