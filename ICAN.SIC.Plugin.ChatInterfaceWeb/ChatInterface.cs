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
            helper = new ChatInterfaceHelper(this);

            ChatApiController.hub = hub;

            string host = System.Configuration.ConfigurationSettings.AppSettings["ChatInterfaceHost"];
            string port = System.Configuration.ConfigurationSettings.AppSettings["ChatInterfacePort"];
            utility.GenerateIndexHtmlFromTemplate(host, port);
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

        public void TestBotResponse(string v)
        {
            helper.AddBotMessage(v);
        }
    }
}
