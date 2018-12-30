using ICAN.SIC.Abstractions.IMessageVariants;
using Syn.Bot.Siml;
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

    //public class BotResult : IBotResult
    //{
    //    IUserResponse userResponse;
    //    string text;

    //    public BotResult(string text, IUserResponse userResponse)
    //    {
    //        this.text = text;
    //        this.userResponse = userResponse;
    //    }

    //    public string Text { get { return this.text; } }

    //    public ChatResult ChatResult { get { return null; } }

    //    public IUserResponse UserResponse { get { return userResponse; } }
    //}
}
