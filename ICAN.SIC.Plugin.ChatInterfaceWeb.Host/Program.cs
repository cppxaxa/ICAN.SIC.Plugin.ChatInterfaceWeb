using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAN.SIC.Plugin.ChatInterfaceWeb.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatInterface chat = new ChatInterface();
            Console.ReadKey();
            chat.TestBotResponse("Hello User");
            Console.WriteLine("Exit: ");
        }
    }
}
