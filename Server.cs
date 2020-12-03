using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPServer
{
    class Server
    {

        private UdpClient server;
        private IPEndPoint endPoint;

        private int retransmissionsLimit = 5;

        private byte[] buffer;
        private byte[] serverBuffer;


        public Server()
        {
            server = new UdpClient(Service.serverPort);
            endPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public void SendRequest(string input)
        {
            var message = RefactorUserInput(input);

            buffer = Encoding.Unicode.GetBytes(message);

            server.Send(buffer, buffer.Length, Service.hostName, Service.clientPort);

            CheckResponse();
        }

        public string ReceiveResponse()
        {
            buffer = server.Receive(ref endPoint);

            string clientMessage = Encoding.Unicode.GetString(buffer);

            if (clientMessage != "nack" && clientMessage != "ack") 
            { 

                bool isValid = MessageChecker(clientMessage);

                if (!isValid)
                    ResendMessage();

                if (isValid)
                    SendAck();
                return clientMessage;
            }
            else
            {
                return "none";
            }

        }

        private void ResendMessage()
        {
            buffer = Encoding.Unicode.GetBytes("nack");

            server.Send(buffer, buffer.Length, Service.hostName, Service.clientPort);
        }

        private void SendAck()
        {
            buffer = Encoding.Unicode.GetBytes("ack");

            server.Send(buffer, buffer.Length, Service.hostName, Service.clientPort);
        }
        private bool MessageChecker(string clientMessage)
        {

            if (string.IsNullOrEmpty(clientMessage))
                return false;
            JObject json = JObject.Parse(clientMessage);

            int clientHash = int.Parse(json["hash"].ToString());

            int hash = json["message"].ToString().GetHashCode();

            if (clientHash != hash)
                return false;

            return true;
        }

        private string RefactorUserInput(string clientInput)
        {
            var hash = clientInput.GetHashCode();

            string message = "{\"hash\":" + $"\"{hash}\"," +
                                    $"\"message\": \"{clientInput}\"" + "}";
            return message;
        }

        private void CheckResponse()
        {
            endPoint = new IPEndPoint(IPAddress.Any, 0);
            serverBuffer = server.Receive(ref endPoint);

            CheckMessageValidation(serverBuffer, buffer);
        }
        private void CheckMessageValidation(byte[] serverBuffer, byte[] buffer)
        {
            var message = Encoding.Unicode.GetString(serverBuffer);

            for (int counter = 0; counter < retransmissionsLimit; counter++)
            {
                if (message == "nack")
                {
                    server.Send(buffer, buffer.Length, Service.hostName, Service.clientPort);
                    //Console.WriteLine("error");
                }
                if (message == "ack")
                {
                    //Console.WriteLine("Good Requst");
                    break;
                }
            }
        }
    }
}
