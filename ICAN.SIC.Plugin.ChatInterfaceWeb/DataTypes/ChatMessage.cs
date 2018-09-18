using ICAN.SIC.Abstractions.IMessageVariants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAN.SIC.Plugin.ChatInterfaceWeb.DataTypes
{
    public class ChatMessage : IChatMessage
    {
        public ChatMessage()
        {

        }

        public ChatMessage(string markupContent)
        {
            this.markupContent = markupContent;
        }

        public string MarkupContent
        {
            get { return markupContent; }
            set { this.markupContent = value; }
        }

        private string markupContent;
    }
}
