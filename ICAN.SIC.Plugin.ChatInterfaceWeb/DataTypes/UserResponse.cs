using ICAN.SIC.Abstractions.IMessageVariants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAN.SIC.Plugin.ChatInterfaceWeb
{
    public class UserResponse : IUserResponse
    {
        string text;

        public UserResponse(string Text)
        {
            this.text = Text;
        }

        public string Text { get { return this.text; } }
    }
}
