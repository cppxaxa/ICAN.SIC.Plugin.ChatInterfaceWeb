using ICAN.SIC.Abstractions;
using ICAN.SIC.Abstractions.IMessageVariants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ICAN.SIC.Plugin.ChatInterfaceWeb
{
    public class MachineMessageApiController : ApiController
    {
        public static IHub hub;

        // GET MachineMessageApi 
        public IEnumerable<string> Get()
        {
            return new string[] { "GET /MachineMessageApi Get this help", "POST /MachineMessageApi : Post a IMachineMessage" };
        }

        // POST MachineMessageApi 
        public string Post([FromBody]string value)
        {
            if (hub == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("MachineMessageApiController error, hub is not available. IMachineMessage not pushed with message: {0}", value);
                Console.ResetColor();
                return "Server Api Error";
            }

            IMachineMessage userResponse = new MachineMessage(value);
            hub.Publish<IMachineMessage>(userResponse);

            Console.WriteLine("[INFO] /MachineMessageApi: MachineMessage published: {0}", value);

            return "Acknowledged";
        }
    }
}
