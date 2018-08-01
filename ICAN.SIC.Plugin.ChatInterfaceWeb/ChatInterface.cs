using ICAN.SIC.Abstractions;
using ICAN.SIC.Abstractions.IMessageVariants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAN.SIC.Plugin.ChatInterface
{
    public class ChatInterface : AbstractPlugin, IChatInterface
    {
        ChatInterfaceHelper helper;
        ChatInterfaceUtility utility = new ChatInterfaceUtility();

        public ChatInterface() : base("ChatInterface")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[INFO] Hello from ChatInterface");
            Console.ResetColor();

            hub.Subscribe<IBotResult>(this.AddBotResult);
            hub.Subscribe<IUserResponse>(this.AddUserResponse);
            helper = new ChatInterfaceHelper(this);
        }

        private void AddUserResponse(IUserResponse response)
        {
            helper.AddUserMessage(response.Text);
        }

        private void AddBotResult(IBotResult response)
        {
            helper.AddBotMessage(response.Text);
        }

        public void PushUserResponse(string message)
        {
            IUserResponse response = new UserResponse(message);
            hub.Publish<IUserResponse>(response);
        }

        public void TestBotResponse(string v)
        {
            helper.AddBotMessage(v);
        }
    }
}
