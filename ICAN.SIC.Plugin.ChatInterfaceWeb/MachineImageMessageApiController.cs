using ICAN.SIC.Abstractions;
using ICAN.SIC.Abstractions.IMessageVariants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;


using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ICAN.SIC.Plugin.ChatInterfaceWeb
{
    public class MachineImageMessageApiController : ApiController
    {
        public static IHub hub;

        // GET MachineImageMessageApi 
        public IEnumerable<string> Get()
        {
            return new string[] { "GET /MachineImageMessageApi Get this help", "POST /MachineImageMessageApi : Post a IMachineImageMessage" };
        }

        // POST MachineImageMessageApi 
        public string Post()
        {
            var stream = new MemoryStream();
            this.ControllerContext.Request.Content.CopyToAsync(stream).Wait();

            stream.Seek(0, SeekOrigin.Begin);

            HttpMultipartParser.MultipartFormDataParser parser = new HttpMultipartParser.MultipartFormDataParser(stream);
            var filesList = parser.Files;

            if (filesList.Count > 0)
            {
                string uploadsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
                if (!Directory.Exists(uploadsDirectory))
                    Directory.CreateDirectory(uploadsDirectory);


                string resourceIdForImage = filesList.First().FileName;
                Stream data = filesList.First().Data;

                bool ImageParsedSuccessfully = false;
                try
                {
                    data.Seek(0, SeekOrigin.Begin);
                    Image image = Image.FromStream(data);
                    image.Save(Path.Combine(uploadsDirectory, resourceIdForImage));
                    ImageParsedSuccessfully = true;


                    // Publish IMachineImageMessage here only

                }
                catch {
                    data.Seek(0, SeekOrigin.Begin);
                    FileStream fileStream = new FileStream(Path.Combine(uploadsDirectory, resourceIdForImage), FileMode.Create);
                    data.CopyTo(fileStream);
                    fileStream.Close();
                }

                if (hub == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("MachineImageMessageApiController error, hub is not available. IMachineMessage not pushed with message");
                    Console.ResetColor();
                    return "Server Api Error";
                }

                // Posting the information as IMachineMessage
                IMachineMessage message = new MachineMessage(string.Format("MachineImageMessage published: {0}, Image parsing done: {1}", resourceIdForImage, ImageParsedSuccessfully));
                hub.Publish<IMachineMessage>(message);

                Console.WriteLine("[INFO] /MachineImageMessageApi: MachineImageMessage published: {0}, Image parsing done: {1}", resourceIdForImage, ImageParsedSuccessfully);
            }

            return "Acknowledged";
        }
    }
}
