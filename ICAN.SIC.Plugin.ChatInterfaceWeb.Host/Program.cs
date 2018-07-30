using System;
using System.Collections.Generic;
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

            Assembly assembly = Assembly.LoadFrom(@"D:\Projects\ICAN\ICAN.SIC.Plugin.ChatInterfaceWeb\bin\Debug\ICAN.SIC.Plugin.ChatInterfaceWeb.dll");

            ChatInterface.ChatInterface chatInterface = (ChatInterface.ChatInterface)assembly.CreateInstance("ICAN.SIC.Plugin.ChatInterface.ChatInterface");
            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
