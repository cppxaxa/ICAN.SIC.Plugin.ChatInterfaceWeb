using ICAN.SIC.Abstractions;
using ICAN.SIC.Abstractions.IMessageVariants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ICAN.SIC.Plugin.ChatInterfaceWeb.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            //ChatInterface chat = new ChatInterface();
            //Console.ReadKey();
            //chat.TestBotResponse("Hello User");
            //Console.WriteLine("Exit: ");

            Assembly assembly = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ICAN.SIC.Plugin.ChatInterfaceWeb.dll"));

            IPlugin chatInterfacePlugin = (IPlugin)assembly.CreateInstance("ICAN.SIC.Plugin.ChatInterface.ChatInterface");
            ChatInterface.ChatInterface chatInterface = (ChatInterface.ChatInterface)chatInterfacePlugin;
            
            chatInterface.Hub.Subscribe<IMachineMessage>(PrintMachineMessage);

            Console.WriteLine("Press any key to continue ...");
            Console.ReadLine();

            chatInterface.PushUserResponse("User message");
            chatInterface.TestBotResponse("Bot message");
            chatInterface.TestChatMessage("<b>ABC</b>");

            ChatMessage sample = new ChatMessage("<svg width=\"100\" height=\"100\"><circle cx=\"50\" cy=\"50\" r=\"40\" stroke=\"green\" stroke-width=\"4\" fill=\"yellow\" /></svg>");
            chatInterface.Hub.Publish<IChatMessage>(sample);


            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static void PrintMachineMessage(IMachineMessage message)
        {
            Console.WriteLine("[INFO] IMachineMessage: " + message.Message);
        }
    }
}
