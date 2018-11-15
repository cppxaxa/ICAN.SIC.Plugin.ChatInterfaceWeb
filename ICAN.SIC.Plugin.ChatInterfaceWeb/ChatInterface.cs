using ICAN.SIC.Abstractions;
using ICAN.SIC.Abstractions.IMessageVariants;
using ICAN.SIC.Plugin.ChatInterfaceWeb;
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
            hub.Subscribe<ILog>(this.AddInfoLog);
            hub.Subscribe<IChatMessage>(this.ParseChatMessage);
            hub.Subscribe<IMachineMessage>(this.AddMachineMessage);
            hub.Subscribe<IUserFriendlyMachineMessage>(this.AddUserFriendlyMachineMessage);
            
            helper = new ChatInterfaceHelper(this);

            ChatApiController.hub = hub;
            MachineMessageApiController.hub = hub;
            MachineImageMessageApiController.hub = hub;

            string host = System.Configuration.ConfigurationSettings.AppSettings["ChatInterfaceHost"];
            string port = System.Configuration.ConfigurationSettings.AppSettings["ChatInterfacePort"];
            utility.GenerateIndexHtmlFromTemplate(host, port);
        }

        private void AddMachineImageMessage(IMachineImageMessage message)
        {
            helper.AddMachineImageMessage(message.Filename, message.NetworkLocalHttpUri, message.Image);
        }

        private void AddUserFriendlyMachineMessage(IUserFriendlyMachineMessage message)
        {
            Console.WriteLine("User Friendly Machine Message added: " + message.PrettyMessage);
            helper.AddUserFriendlyMachineMessage(message.PrettyMessage);
        }

        private void AddMachineMessage(IMachineMessage machineMessage)
        {
            helper.AddMachineMessage(machineMessage.Message);
        }

        private void ParseChatMessage(IChatMessage chatMessage)
        {
            helper.AddChatMessage(chatMessage.MarkupContent);
        }

        private void AddUserResponse(IUserResponse response)
        {
            helper.AddUserMessage(response.Text);
        }

        private void AddInfoLog(ILog response)
        {
            string prefix = String.Empty;
            switch(response.LogType)
            {
                case LogType.Debug:
                    prefix = "[DEBUG] ";
                    break;
                case LogType.Error:
                    prefix = "[ERROR] ";
                    break;
                case LogType.Info:
                    prefix = "[INFO] ";
                    break;
                case LogType.Warning:
                    prefix = "[WARNING] ";
                    break;
            }
            helper.AddInfoLog(prefix + response.Message);
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

        public void PushMachineMessage(string message)
        {
            IMachineMessage machineMessage = new MachineMessage(message);
            hub.Publish<IMachineMessage>(machineMessage);
        }

        public void TestBotResponse(string v)
        {
            helper.AddBotMessage(v);
        }

        public void TestUserMessage(string v)
        {
            helper.AddUserMessage(v);
        }

        public void TestChatMessage(string v)
        {
            helper.AddChatMessage(v);
        }
    }
}
