using ICAN.SIC.Abstractions.IMessageVariants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICAN.SIC.Plugin.ChatInterface
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

    //public class BotResponse: ICAN.SIC.Abstractions.IMessageVariants.IBotResult
    //{
    //    string text;

    //    public BotResponse(string text)
    //    {
    //        this.text = text;
    //    }

    //    public string Text { get { return this.text; } }
    //}
}
