using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.Owin.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using System.Web.Http;
using ICAN.SIC.Abstractions.IMessageVariants;
using System.Drawing;
using System.Drawing.Imaging;

namespace ICAN.SIC.Plugin.ChatInterface
{
    public class ChatInterfaceHelper
    {
        IHubContext signalRHub;
        IChatInterface chatInterface;
        ChatInterfaceUtility utility = new ChatInterfaceUtility();

        public ChatInterfaceHelper(IChatInterface chatInterface)
        {
            this.chatInterface = chatInterface;
            Startup.chatInterface = chatInterface;

            string host = System.Configuration.ConfigurationSettings.AppSettings["ChatInterfaceHost"];
            string port = System.Configuration.ConfigurationSettings.AppSettings["ChatInterfacePort"];
            
            Debug.Assert(port != null, "Please add a key to configuration as follows:" +
                                    "<configuration>" +
                                      "<runtime>" +
                                        "<appSettings>" +
                                          "<add key = \"ChatInterfaceHost\" value = \"localhost\" />" +
                                          "<add key = \"ChatInterfacePort\" value = \"20000\" />" +
                                        "</appSettings>" +
                                      "</runtime>" +
                                    "</configuration>");

            string url = "http://" + host + ":" + port;
            WebApp.Start<Startup>(url);
            signalRHub = GlobalHost.ConnectionManager.GetHubContext<ChatInterfaceSignalRHub>();
            Debug.Assert(signalRHub != null, "No chatInterface signalR running hub found");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("[INFO] HTTP ChatInterface started at ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0}/ChatInterface", url);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("[INFO] HTTP ChatApi started at ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0}/ChatApi", url);
            Console.ResetColor();
            Console.WriteLine("[INFO] GET /ChatApi : Help menu");
            Console.WriteLine("[INFO] POST /ChatApi : Post an IUserResponse");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("[INFO] HTTP MachineMessageApi started at ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0}/MachineMessageApi", url);
            Console.ResetColor();
            Console.WriteLine("[INFO] GET /MachineMessageApi : Help menu");
            Console.WriteLine("[INFO] POST /MachineMessageApi : Post an IMachineMessage");
        }

        public void AddMachineImageMessage(string filename, string networkLocalHttpUri, Image image)
        {
            string htmlContent = "";

            string jpegEncodedImageString = utility.ImageToBase64(image, ImageFormat.Jpeg);

            List<string> structLines = new List<string>
            {
                "<img style=\"box-shadow: 0px 0px 5px 5px rgba(100, 100, 100, 0.2); margin: 10px; margin-bottom: 14px; border-radius: 5px;\" src=\"",
                "data:image/jpeg;base64,",
                jpegEncodedImageString,
                "\" style=\"max-width: 100%; height: auto;\"></img>"
            };

            foreach (var line in structLines)
            {
                htmlContent += line;
            }

            signalRHub.Clients.All.addChatMessage(htmlContent);
        }

        public void AddBotMessage(string message)
        {
            signalRHub.Clients.All.addBotMessage(message);
        }

        public void AddUserMessage(string message)
        {
            signalRHub.Clients.All.addUserMessage(message);
        }

        public void AddInfoLog(string message)
        {
            signalRHub.Clients.All.addChatMessage(message);
        }

        public void AddChatMessage(string message)
        {
            Console.WriteLine("Chat Message received at ChatInterface");
            signalRHub.Clients.All.addChatMessage(message);
        }

        public void AddMachineMessage(string message)
        {
            signalRHub.Clients.All.addChatMessage("<p style=\"font-size: 12px;\">MACHINE MESSAGE</p> <p style=\"font-size: 12px;\">" + message + "<p>");
        }

        public void AddUserFriendlyMachineMessage(string message)
        {
            signalRHub.Clients.All.addChatMessage("<p style=\"font-size: 12px;\">USER-MACHINE MESSAGE</p> <p style=\"font-size: 12px;\">" + message + "<p>");
        }

        class Startup
        {
            public static IChatInterface chatInterface;

            public void Configuration(IAppBuilder app)
            {
                app.UseCors(CorsOptions.AllowAll);

                GlobalHost.DependencyResolver.Register(
                    typeof(ChatInterfaceSignalRHub),
                    () => new ChatInterfaceSignalRHub(chatInterface));


                app.MapSignalR();

                PhysicalFileSystem physicalFileSystem = new PhysicalFileSystem("WebAssets");
                FileServerOptions options = new FileServerOptions
                {
                    EnableDefaultFiles = true,
                    FileSystem = physicalFileSystem
                };
                options.StaticFileOptions.FileSystem = physicalFileSystem;
                options.StaticFileOptions.ServeUnknownFileTypes = true;
                options.DefaultFilesOptions.DefaultFileNames = new[] { "index.html", "Default.html" };
                app.UseFileServer(options);

                HttpConfiguration config = new HttpConfiguration();
                config.Routes.MapHttpRoute(
                    name: "ChatController",
                    routeTemplate: "{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
                app.UseWebApi(config);
            }
        }
        public class ChatInterfaceSignalRHub : Microsoft.AspNet.SignalR.Hub
        {
            IChatInterface chatInterface;

            public ChatInterfaceSignalRHub(IChatInterface chatInterface)
            {
                this.chatInterface = chatInterface;
            }

            public void AddBotMessage(string message)
            {
                Clients.All.addBotMessage(message);
            }

            public void AddInfoLog(string message)
            {
                Clients.All.addUIInfoLogMessage(message);
            }

            public void AddChatMessage(string message)
            {
                Clients.All.addChatMessage(message);
            }

            public void ProcessUserMessage(string message)
            {
                chatInterface.PushUserResponse(message);
            }

            public void AddMachineMessage(string message)
            {
                Clients.All.addUIInfoLogMessage("MACHINE MESSAGE: <p style=\"font-size: 11px;\">" + message + "<p>");
            }

            public void AddUserFriendlyMachineMessage(string message)
            {
                Clients.All.addUIInfoLogMessage("USER-MACHINE MESSAGE: <p style=\"font-size: 11px;\">" + message + "<p>");
            }
        }
    }
}
