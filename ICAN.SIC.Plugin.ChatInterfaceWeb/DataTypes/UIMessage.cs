using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAN.SIC.Plugin.ChatInterfaceWeb
{
    class UIMessage : IUIMessage
    {
        Color color;
        string message;

        public UIMessage(Color Color, string Message)
        {
            this.color = Color;
            this.message = Message;
        }

        public Color Color { get { return this.color; } }
        public string Message { get { return this.message; } }
    }
}
