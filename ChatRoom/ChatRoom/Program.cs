using System;
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
            Client client = new Client("127.0.0.1", 9999);

            //New thread asking for user input to send
            client.Send();
            //New thread recieving network messages
            client.Recieve();

            Console.ReadLine();
        }
    }
}
