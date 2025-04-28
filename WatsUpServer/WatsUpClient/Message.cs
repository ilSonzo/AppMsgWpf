using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatsUpClient
{
    public class Message
    {
        public string MsgType { get; set; }
        public string Content { get; set; }
        public DateTime DateOfMsg { get; set; }
        public string Sender { get; set; }

        public Message(string t, string c, DateTime d, string s)
        {
            t.ToLower();
            MsgType = t;
            Content = c;
            DateOfMsg = d;
            Sender = s;
        }

        public Message() { }

        public override string ToString()
        {
            return "[" + MsgType + "|" + DateOfMsg.ToString() + "|" + Sender +"]" + Content;
        }


    }
}
