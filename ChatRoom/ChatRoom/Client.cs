using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom
{
    class Client
    {
        TcpClient clientSocket;
        NetworkStream stream;
        string userName;

        public string UserName { get { return userName; } set { userName = value; } }

        public Client(string IP, int port)
        {
            //Create Setup method for the following, dont put it in the constructor
            userName = UI.GetUserName();
            clientSocket = new TcpClient();
            try
            {
                clientSocket.Connect(IPAddress.Parse(IP), port);
                stream = clientSocket.GetStream();
                byte[] userNameMessage = Encoding.ASCII.GetBytes(userName);
                stream.Write(userNameMessage, 0, userNameMessage.Count());
            }
            catch (Exception)
            {
                Console.WriteLine("Server Not Found");
            }
        }
        public void Send()
        {
            string messageString = UI.GetInput();
            byte[] message = Encoding.ASCII.GetBytes(messageString);
            stream.Write(message, 0, message.Count());
        }
        public void Recieve()
        {
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            UI.DisplayMessage(Encoding.ASCII.GetString(recievedMessage));
        }
    }
}
