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

namespace ICAN.SIC.Plugin.ChatInterfaceWeb
{
    public class ChatInterfaceHelper
    {
        IHubContext signalRHub;

        public ChatInterfaceHelper()
        {
            string port = System.Configuration.ConfigurationSettings.AppSettings["ChatInterfacePort"];

            Debug.Assert(port != null, "Please add a key to configuration as follows:" +
                                    "<configuration>" +
                                      "<runtime>" +
                                        "<appSettings>" +
                                          "<add key = \"Port\" value = \"20000\" />" +
                                        "</appSettings>" +
                                      "</runtime>" +
                                    "</configuration> ");

            string url = "http://localhost:" + port;
            WebApp.Start<Startup>(url);
            signalRHub = GlobalHost.ConnectionManager.GetHubContext<ChatInterfaceSignalRHub>();
            Debug.Assert(signalRHub != null, "No chatInterface signalR running hub found");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[INFO] HTTP ChatInterface startet at {0}/", url);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void AddBotMessage(string message)
        {
            signalRHub.Clients.All.addBotMessage(message);
        }

        class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.UseCors(CorsOptions.AllowAll);

                app.MapSignalR();

                PhysicalFileSystem physicalFileSystem = new PhysicalFileSystem("WebAssets");
                FileServerOptions options = new FileServerOptions
                {
                    EnableDefaultFiles = true,
                    FileSystem = physicalFileSystem
                };
                options.StaticFileOptions.FileSystem = physicalFileSystem;
                //options.StaticFileOptions.ServeUnknownFileTypes = true;
                options.DefaultFilesOptions.DefaultFileNames = new[] { "index.html", "Default.html" };
                app.UseFileServer(options);
            }
        }
        public class ChatInterfaceSignalRHub : Microsoft.AspNet.SignalR.Hub
        {
            public void AddBotMessage(string message)
            {
                Clients.All.addBotMessage(message);
            }
        }
    }
}
