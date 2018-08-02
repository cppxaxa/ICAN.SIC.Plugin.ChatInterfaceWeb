using ICAN.SIC.Abstractions;
using ICAN.SIC.Plugin.ChatInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ICAN.SIC.Plugin.ChatInterfaceWeb
{
    public class ChatApiController : ApiController
    {
        public static IHub hub;

        // GET ChatApi 
        public IEnumerable<string> Get()
        {
            return new string[] { "GET /ChatApi Get this help", "POST /ChatApi : Post a IUserResponse" };
        }

        // POST ChatApi 
        public string Post([FromBody]string value)
        {
            if (hub == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ChatApiController error, hub is not available. IUserResponse not pushed with message: {0}", value);
                Console.ResetColor();
                return "Server Api Error";
            }

            UserResponse userResponse = new UserResponse(value);
            hub.Publish<UserResponse>(userResponse);

            Console.Write("[INFO] /ChatApi: UserResponse published: {0}", value);

            return "Acknowledged";
        }
    }
}
