﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            client.SetupClient("127.0.0.1", 9999);
            Console.WriteLine("Anykey to continue...");
            Console.ReadLine();
        }
    }
}
