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
using ICAN.SIC.Plugin.ChatInterfaceWeb.DataTypes;
using System.Threading;
using System.Net;
using MimeTypes;

namespace ICAN.SIC.Plugin.ChatInterfaceWeb
{
    public class MachineImageMessageApiController : ApiController
    {
        public static IHub hub;
        string uploadRelativeDirectory = @"WebAssets\Uploads";

        // GET MachineImageMessageApi 
        public IEnumerable<string> Get()
        {
            return new string[] { "GET /MachineImageMessageApi Get this help", "POST /MachineImageMessageApi : Post a IMachineImageMessage" };
        }

        // GET MachineImageMessageApi 
        public HttpResponseMessage Get(string id)
        {
            string uploadsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, uploadRelativeDirectory);

            if (Directory.Exists(uploadsDirectory))
                foreach (var file in Directory.GetFiles(uploadsDirectory))
                {
                    string guid = Path.GetFileNameWithoutExtension(file);
                    if (guid.LastIndexOf('_') >= 0 && guid.LastIndexOf('_') < guid.Length)
                    {
                        guid = guid.Substring(guid.LastIndexOf('_') + 1);
                    }
                    else
                    {
                        guid = null;
                    }


                    if (Path.GetFileName(file) == id ||
                        Path.GetFileNameWithoutExtension(file) == id ||
                        (guid != null && guid == id))
                    {
                        byte[] fileBytes = File.ReadAllBytes(file);
                        string ext = Path.GetExtension(file);

                        HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                        result.Content = new ByteArrayContent(fileBytes);
                        result.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeMap.GetMimeType(ext));

                        string info = string.Format("MachineImageMessageApiController: Serving file \"{0}\"", file);
                        Console.WriteLine("[INFO] " + info);
                        IMachineMessage machineMsg = new MachineMessage(info);
                        hub.Publish<IMachineMessage>(machineMsg);

                        return result;
                    }
                }

            HttpResponseMessage errorMessage = new HttpResponseMessage(HttpStatusCode.OK);
            errorMessage.Content = new ByteArrayContent(Encoding.ASCII.GetBytes("No image found"));
            errorMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/json");

            string information = string.Format("MachineImageMessageApiController: No file found");
            Console.WriteLine("[INFO] " + information);
            IMachineMessage machineMessage = new MachineMessage(information);
            hub.Publish<IMachineMessage>(machineMessage);

            return errorMessage;
        }

        // POST MachineImageMessageApi 
        public string Post()
        {
            var stream = new MemoryStream();
            ControllerContext.Request.Content.CopyToAsync(stream).Wait();

            stream.Seek(0, SeekOrigin.Begin);

            HttpMultipartParser.MultipartFormDataParser parser = new HttpMultipartParser.MultipartFormDataParser(stream);
            var filesList = parser.Files;

            if (filesList.Count > 0)
            {
                string uploadsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, uploadRelativeDirectory);
                if (!Directory.Exists(uploadsDirectory))
                    Directory.CreateDirectory(uploadsDirectory);

                Guid guid = Guid.NewGuid();
                string resourceIdForImage = filesList.First().FileName;
                resourceIdForImage = Path.GetFileNameWithoutExtension(resourceIdForImage) + "_" + guid.ToString() + Path.GetExtension(resourceIdForImage);
                Stream data = filesList.First().Data;

                bool ImageParsedSuccessfully = false;
                try
                {
                    data.Seek(0, SeekOrigin.Begin);
                    Image image = Image.FromStream(data);
                    image.Save(Path.Combine(uploadsDirectory, resourceIdForImage));
                    ImageParsedSuccessfully = true;

                    // Publish IMachineImageMessage here only
                    string host = System.Configuration.ConfigurationSettings.AppSettings["ChatInterfaceHost"];
                    string port = System.Configuration.ConfigurationSettings.AppSettings["ChatInterfacePort"];
                    string networkLocalHttpUri = "http://" + host + ":" + port + "/MachineImageMessageApi/" + resourceIdForImage;
                    string deviceLocalFilePath = Path.Combine(uploadsDirectory, resourceIdForImage);
                    MachineImageMessage imageMessage = new MachineImageMessage(resourceIdForImage, image, networkLocalHttpUri, deviceLocalFilePath);
                    hub?.Publish<IMachineImageMessage>(imageMessage);
                }
                catch
                {
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
                hub?.Publish<IMachineMessage>(message);

                Console.WriteLine("[INFO] /MachineImageMessageApi: MachineImageMessage published: {0}, Image parsing done: {1}", resourceIdForImage, ImageParsedSuccessfully);



                return resourceIdForImage;
            }

            return "Acknowledged";
        }
    }
}
