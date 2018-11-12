using ICAN.SIC.Abstractions.IMessageVariants;
using System.Drawing;

namespace ICAN.SIC.Plugin.ChatInterfaceWeb.DataTypes
{
    public class MachineImageMessage : IMachineImageMessage
    {
        public MachineImageMessage(string filename, Image image)
        {
            this.Filename = filename;
            this.Image = image;
        }

        public MachineImageMessage()
        {  }

        public string Filename { get; set; }
        public Image Image { get; set; }
    }
}
