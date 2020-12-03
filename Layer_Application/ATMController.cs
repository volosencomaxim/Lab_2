using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPServer.Layer_Application
{
    class ATMController
    {
        ServerEncryption transport;
        readonly List<UserModel> data;
        UserModel dude;


        public ATMController()
        {
            data = GenerateData();
            transport = new ServerEncryption();
            transport.CreateConnection();
            PINCodeChecker();
        }

        private List<UserModel> GenerateData()
        {
            List<UserModel> dataSet = new List<UserModel>();

            dataSet.Add(new UserModel { Name = "Thomas", PIN = 5123, Balance = 12451.01f });
            dataSet.Add(new UserModel { Name = "Steven", PIN = 1234, Balance = 9319301.13f });
            dataSet.Add(new UserModel { Name = "Justin", PIN = 8470, Balance = 3123.01f });
            dataSet.Add(new UserModel { Name = "Kyle", PIN = 6842, Balance = 120.13f });
            dataSet.Add(new UserModel { Name = "Noah", PIN = 1468, Balance = 3123.91f });
            dataSet.Add(new UserModel { Name = "Bradley", PIN = 9436, Balance = 31234.51f });
            dataSet.Add(new UserModel { Name = "Eugene", PIN = 4383, Balance = 832.01f });

            return dataSet;
        }

        private void PINCodeChecker()
        {
            string welcomeWord = "Welcome to ATM\n" +
                                 "Enter your PIN: ";

            transport.SendRequest(welcomeWord);
            var enteredPin = int.Parse(transport.ReceiveResponse());

            Console.Write(welcomeWord);
            //var enteredPin = int.Parse(Console.ReadLine());

            dude = data.Where(p => p.PIN == enteredPin).FirstOrDefault();

            if (dude != null)
            {


                DisplayOptions();
            }
            else
            {
                string wrongPIN = "Wrong PIN Code";

                transport.SendRequest(wrongPIN);


                Console.WriteLine(wrongPIN);
                PINCodeChecker();
            }

        }
        private void DisplayOptions(string name = null, string addOn = null)
        {

            string options = "\n1.Balance\n" +
                                "2.Withdraw\n" +
                                "3.Deposit\n" +
                                "4.Quit\n";

            if (addOn != null)
            {
                addOn += options;
                transport.SendRequest(addOn);
            }
            else
            {
                transport.SendRequest(options);
            }

            Console.WriteLine(options);

            //var choice = int.Parse(Console.ReadLine());
            var choice = int.Parse(transport.ReceiveResponse());

            OperationsManager(choice);
        }

        private void OperationsManager(int option)
        {
            switch (option) {

                case 1:
                    string opt1 = "You have: " + dude.Balance + "\n";

                    //transport.SendRequest(opt1);
                    Console.WriteLine(opt1);
                    DisplayOptions(addOn : opt1);
                break;

                case 2:
                    var opt2 = Option2();
                    DisplayOptions(addOn: opt2);
                break;

                case 3:
                    var opt3 = Option3();
                  DisplayOptions(addOn: opt3);
                break;

                case 4:
                    string opt4 = "Have a nice day";
                    transport.SendRequest(opt4);
                    Console.WriteLine(opt4);
                break;

                default:
                    string tryAgain = "Wrong. Try again.";
                    //transport.SendRequest(tryAgain);
                    Console.WriteLine(tryAgain);
                    DisplayOptions(addOn: tryAgain);
                break;
            }
        }
        private string Option2()
        {
            string amountToWin = "Enter amount: \n ";
            transport.SendRequest(amountToWin);
            Console.Write(amountToWin);

            var moneyExtracted = int.Parse(transport.ReceiveResponse());

            if (dude.Balance - moneyExtracted < 0)
            {
                string notEnoght = "You don't have enough money\n";
                //transport.SendRequest(notEnoght);
                
                Console.WriteLine(notEnoght);
                return notEnoght;
            }
            else
            {
                dude.Balance = dude.Balance - moneyExtracted;
                string amountLeft = $"{dude.Balance} left\n\n";
                //transport.SendRequest(amountLeft);

                Console.WriteLine(amountLeft);
                return amountLeft;
            }
        }

        private string Option3()
        {
            string amountToDeposit = "Enter how much money you want to deposit : ";

            transport.SendRequest(amountToDeposit);


            Console.Write(amountToDeposit);
            var moneyDeposited = int.Parse(transport.ReceiveResponse());


            dude.Balance = dude.Balance + moneyDeposited;

            string inBalance = $"{dude.Balance} left";
            //transport.SendRequest(inBalance);

            Console.WriteLine(inBalance);

            return inBalance;
            
        }
    }
}
