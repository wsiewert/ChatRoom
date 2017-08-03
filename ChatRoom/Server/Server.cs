﻿using System;
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
        Queue<string> messages = new Queue<string>();
        TcpListener server;
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

            //add client to dictionary
            string userId = clientDictionary.Count.ToString();
            newClient.UserId = userId;
            clientDictionary.Add(newClient.UserId, newClient);
            Console.WriteLine(newClient.userName + "[Logged In]");

            //start new "check receiving" task here, NOT inside the client object.
            //Task client.recieving...
            Task clientMessageReceiving = new Task(() => AddClientMessageToQueue(newClient));
            clientMessageReceiving.Start();
        }

        private void AddClientMessageToQueue(Client client)
        {
            //try-catch, if exception thrown, exit function(results in exit of task) and console.write("client disconnected");
            //use removeClientByUserName() to remove from dictionary
        }

        private void GetMessageFromQueue()
        {
            
        }

        private void Respond(string body)
        {
            //client.Send(body);
            //make into async task. UpdateMessageQueue();
            //try catch per client to see if someone disconnected during a message broadcast, then delete person from dictionary
        }

        private void RemoveClientByUserName()
        {

        }
    }
}
