using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPServer
{
    class ServerEncryption
    {
        private Server _udpServer;
        private RSAEncryption encry;
        private bool _isSecured = false;
        private string publicData;
        private string encryptMessage;


        public ServerEncryption(/*Server udpServer*/)
        {
            _udpServer = new Server();
            encry = new RSAEncryption();

            //CreateConnection(udpServer);
        }

        public void CreateConnection(/*Server udpServer*/)
        {
            while (!_isSecured)
            {
                publicData = _udpServer.ReceiveResponse();
                _udpServer.SendRequest(encry.PublicKeyString());
                _isSecured = true;
            }
        }

        public string ReceiveResponse()
        {
            encryptMessage = _udpServer.ReceiveResponse();

            return DecryptMessage();
        }

        public void SendRequest(string input)
        {
            string[] data = ExtractMessage(publicData).Split('.');

            var publicKey = encry.SetPublicKey(data[0], data[1]);

            var encryptedInput = encry.Encrypt(publicKey, input);

            _udpServer.SendRequest(encryptedInput);
        }
        private string ExtractMessage(string input)
        {
            JObject json = JObject.Parse(input);

            return json["message"].ToString();
        }

        public string DecryptMessage()
        {
            string decryptMessage = encry.Decrypt(ExtractMessage(encryptMessage));

            //Console.WriteLine(decryptMessage);
            return decryptMessage;
        }
    }
}
