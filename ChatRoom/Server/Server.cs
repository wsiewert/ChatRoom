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
        //public static Client client;
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
            //Create threads to run accept clients and message broadcasting
            Task clientAccept = new Task(() => GetClients());
            clientAccept.Start();

            //string message = client.Recieve();
            Respond("[Placeholder]");
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
            Console.WriteLine("Connected");
            NetworkStream stream = clientSocket.GetStream();
            Client newClient = new Client(stream, clientSocket);

            //change generate userID using uct time.
            string userId = clientConnectionsCount.ToString();
            newClient.UserId = userId;
            clientDictionary.Add(newClient.UserId, newClient);
            Console.WriteLine(newClient.userName + "[Logged In]");
            clientConnectionsCount++;

            //Task client.recieving...
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
                    BroadcastNewMessage(clientMessage);
                }
                catch (Exception)
                {
                    clientConnected = false;
                    RemoveClient(client.UserId,client);
                }
            }
        }

        private void AddMessageToQueue(string message)
        {
            messageQueue.Enqueue(message);
        }

        private void BroadcastNewMessage(string clientMessage)
        {
            foreach (KeyValuePair<string, Client> client in clientDictionary)
            {
                client.Value.Send(clientMessage);
            }
        }

        private string GetMessageFromQueue()
        {
            return messageQueue.Peek();
        }

        private void Respond(string body)
        {
            //client.Send(body);
            //make into async task. UpdateMessageQueue();
            //try catch per client to see if someone disconnected during a message broadcast, then delete person from dictionary
        }

        private void RemoveClient(string userId, Client client)
        {
            //remove user from dictionary, broadcast disconnection
            string clientDisconnectedMessage = client.userName + "[Disconnected]";
            AddMessageToQueue(clientDisconnectedMessage);
            BroadcastNewMessage(clientDisconnectedMessage);
            Console.WriteLine(clientDisconnectedMessage);
            clientDictionary.Remove(userId);
        }
    }
}
