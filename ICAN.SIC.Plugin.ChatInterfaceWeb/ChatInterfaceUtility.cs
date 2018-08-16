using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAN.SIC.Plugin.ChatInterface
{
    public class ChatInterfaceUtility
    {
        public void GenerateIndexHtmlFromTemplate(string host, string port)
        {
            string indexHtml = String.Empty;
            foreach (var line in File.ReadLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"WebAssets" + Path.DirectorySeparatorChar + "ChatInterface" + Path.DirectorySeparatorChar + "indexTemplate.html")))
            {
                string processedLine = line;
                processedLine = processedLine.Replace("{host}", host);
                processedLine = processedLine.Replace("{port}", port);
                indexHtml += processedLine + "\n";
            }

            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"WebAssets" + Path.DirectorySeparatorChar + "ChatInterface" + Path.DirectorySeparatorChar + "index.html"), indexHtml);
        }
    }
}
