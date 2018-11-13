using ICAN.SIC.Abstractions.IMessageVariants;
using System.Drawing;

namespace ICAN.SIC.Plugin.ChatInterfaceWeb.DataTypes
{
    public class MachineImageMessage : IMachineImageMessage
    {
        public MachineImageMessage(string filename, Image image, string networkLocalHttpUri = null, string deviceLocalFilePath = null)
        {
            this.Filename = filename;
            this.Image = image;
            this.NetworkLocalHttpUri = networkLocalHttpUri;
            this.DeviceLocalFilePath = deviceLocalFilePath;
        }

        public MachineImageMessage()
        {  }

        public string Filename { get; set; }
        public Image Image { get; set; }
        public string NetworkLocalHttpUri { get; set; }
        public string DeviceLocalFilePath { get; set; }
    }
}
