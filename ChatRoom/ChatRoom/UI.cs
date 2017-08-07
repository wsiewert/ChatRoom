using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom
{
    public static class UI
    {
        public static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
        public static string GetInput()
        {
            return Console.ReadLine();
        }

        public static string GetUserName()
        {
            Console.WriteLine("Please provide a username:");
            return Console.ReadLine();
        }

        public static void DisplayChatRoomOptions()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Welcome to the ChatRoom");
            Console.WriteLine("Begin typing to chat with others on the server.");
            Console.WriteLine("Enter 'exit' to exit the chatroom and client window");
            Console.ResetColor();
        }
    }
}
