using System;
using UDPServer.Layer_Application;

namespace UDPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";
            Console.WriteLine("Server started ... ");

            //ServerEncryption encryServer = new ServerEncryption();

            //while (true)
            //{ 
            //    encryServer.CreateConnection();

            //    //var clientInput = Console.ReadLine();
            //    //encryServer.SendRequest(clientInput);

            //    encryServer.ReceiveResponse();
            //}

            while (true)
            {
                ATMController atm = new ATMController();
            }

            //atm.GenerateData();

            Console.ReadKey();
        }
    }
}
