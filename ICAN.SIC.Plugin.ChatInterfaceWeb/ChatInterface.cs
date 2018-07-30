using ICAN.SIC.Abstractions;
using ICAN.SIC.Abstractions.IMessageVariants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAN.SIC.Plugin.ChatInterfaceWeb
{
    public class ChatInterface : AbstractPlugin, IChatInterface
    {
        ChatInterfaceHelper helper = new ChatInterfaceHelper();
        ChatInterfaceUtility utility = new ChatInterfaceUtility();

        List<IUIMessage> messageList = new List<IUIMessage>();

        public ChatInterface()
        {
            hub.Subscribe<IUserResponse>(this.AddUserResponse);
            hub.Subscribe<IBotResponse>(this.AddBotResponse);
        }

        private void AddUserResponse(IUserResponse response)
        {
            IUIMessage message = new UIMessage(Color.User, response.Text);
            messageList.Add(message);
        }

        private void AddBotResponse(IBotResponse response)
        {
            IUIMessage message = new UIMessage(Color.Bot, response.Text);
            messageList.Add(message);
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
