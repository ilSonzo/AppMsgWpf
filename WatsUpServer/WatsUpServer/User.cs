using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatsUpServer
{
    [Serializable]
    public class User
    {
        public string Username { get; set; }

        public List<string> Chats { get; set; }

        public User(string username)
        {
            Username = username;
        }

        public User() { }

        public override bool Equals(object obj)
        {
            if(obj != null && obj is User)
                if((obj as User).Username == Username)
                    return true;
            return false;
        }
    }
}
