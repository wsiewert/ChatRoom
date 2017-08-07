using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        Dictionary<string, Client> clientDictionary = new Dictionary<string, Client>();
        Queue<string> messageQueue = new Queue<string>();
        TcpListener server;
        int clientConnectionsCount = 0;

        public Server()
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            server.Start();
        }

        public void Run()
        {
            Task checkNewClients = new Task(() => GetClients());
            checkNewClients.Start();

            Task checkNewMessages = new Task(() => CheckMessageQueue());
            checkNewMessages.Start();
        }

        private void GetClients()
        {
            while (clientDictionary.Count < 5)
            {
                AcceptClient();
            }
        }

        private void AcceptClient()
        {
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            NetworkStream stream = clientSocket.GetStream();
            Client newClient = new Client(stream, clientSocket);

            string userId = clientConnectionsCount.ToString();
            clientConnectionsCount++;
            newClient.UserId = userId;
            clientDictionary.Add(newClient.UserId, newClient);
            string loginMessage = newClient.userName + " [Logged In]";
            AddMessageToQueue(loginMessage);

            Task clientMessageReceiving = new Task(() => CheckIncomingCientMessages(newClient));
            clientMessageReceiving.Start();
        }

        private void CheckIncomingCientMessages(Client client)
        {
            bool clientConnected = true;
            while (clientConnected)
            {
                try
                {
                    string clientMessage = client.Recieve();
                    AddMessageToQueue(clientMessage);
                }
                catch (Exception)
                {
                    clientConnected = false;
                    RemoveClient(client.UserId,client);
                }
            }
        }

        private void CheckMessageQueue()
        {
            while (true)
            {
                //add Lock to client dictionary to stop collisions between tasks:
                //(clientMessageReceiving) & (checkMessageQueue)
                if (messageQueue.Count > 0 && clientDictionary.Count > 0)
                {
                    for (int i = 0; i < messageQueue.Count; i++)
                    {
                            BroadcastNewMessage(messageQueue.Dequeue());
                    }
                }
            }            
        }

        private void AddMessageToQueue(string message)
        {
            messageQueue.Enqueue(message);
            Console.WriteLine(message);
        }

        private void BroadcastNewMessage(string clientMessage)
        {
            foreach (KeyValuePair<string, Client> client in clientDictionary)
            {
                client.Value.Send(clientMessage);
            }
        }

        private void RemoveClient(string userId, Client client)
        {
            string clientDisconnectedMessage = client.userName + " [Disconnected]";
            AddMessageToQueue(clientDisconnectedMessage);
            clientDictionary.Remove(userId);
        }
    }
}
