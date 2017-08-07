using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client
    {
        NetworkStream stream;
        TcpClient client;
        public string UserId;
        public string userName;

        public Client(NetworkStream Stream, TcpClient Client)
        {
            stream = Stream;
            client = Client;
            UserId = "id";

            byte[] recievedUserName = new byte[256];
            stream.Read(recievedUserName, 0, recievedUserName.Length);
            string recievedUserNameString = Encoding.ASCII.GetString(recievedUserName).TrimEnd('\0');
            userName = recievedUserNameString;
        }

        public void Send(string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
        }

        public string Recieve()
        {
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            string recievedMessageString = Encoding.ASCII.GetString(recievedMessage).TrimEnd('\0');
            return "[" + userName + "] " +recievedMessageString;
        }
    }
}
