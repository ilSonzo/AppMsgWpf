using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WatsUpServer
{
    static class GestioneFile
    {
        static public string path_users = "users.xml";
        static public string path_chats = "chats";
        static public string msg_file = "chat.xml";

        public static void SaveUsers(List<User> users)
        {
            try
            {
                XmlSerializer xmls = new XmlSerializer(typeof(List<User>));
                StreamWriter sw = new StreamWriter(path_users);
                xmls.Serialize(sw,users);
                sw.Close();
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }

        public static List<User> ReadUsers()
        {
            List<User> users = new List<User>();
            try
            {
                if (File.Exists(path_users))
                {
                    XmlSerializer xmls = new XmlSerializer(typeof(List<User>));
                    StreamReader sw = new StreamReader(path_users);
                    users = (List<User>)xmls.Deserialize(sw);
                    sw.Close();
                }
            }
            catch (Exception e) { Console.WriteLine("Cannot read users: " + e.ToString()); }
            return users;
        }
        
        public static List<Message> GetUserChat(string user_chat_name)
        {
            string chat_path = Path.Combine(path_chats, user_chat_name, msg_file);
            return GetMessages(chat_path);
        }

        public static string CreateNewChat(string u1, string u2)
        {
            string chat = Path.Combine(path_chats, u1 + "-" + u2);
            Directory.CreateDirectory(chat);
            Directory.CreateDirectory(Path.Combine(chat,"imgs"));
            return u1 + "-" + u2;
        }
        
        public static void AddMessageToChat(string chat, string msg, string type, string user)
        {
            string path = Path.Combine(path_chats, chat);
            if (Directory.Exists(path))
            {
                Message message = new Message(type,msg,DateTime.Now,user);
                SaveOneMsg(message, Path.Combine(path,msg_file));
            }
        }

        public static void AddMessageToChat(string chat, Message msg)
        {
            string path = Path.Combine(path_chats, chat);
            if (Directory.Exists(path))
            {
                SaveOneMsg(msg, Path.Combine(path, msg_file));
            }
        }

        public static List<Message> GetMessages(string path)
        {
            List<Message> msgs = new List<Message>();
            try
            {
                if (File.Exists(path))
                {
                    XmlSerializer xmls = new XmlSerializer(typeof(List<Message>));
                    StreamReader sw = new StreamReader(path);
                    msgs = (List<Message>)xmls.Deserialize(sw);
                    sw.Close();
                }
            }
            catch (Exception e) { Console.WriteLine("Cannot read messages: " + e.ToString()); }
            return msgs;
        }

        public static void SaveOneMsg(Message msg, string path)
        {
            List<Message> messages = GetMessages(path);
            messages.Add(msg);
            try 
            {
                XmlSerializer xmls = new XmlSerializer(typeof(List<Message>));
                StreamWriter sw = new StreamWriter(path);
                xmls.Serialize(sw, messages);
                sw.Close();
            }
            catch (Exception e) { Console.WriteLine("Cannot save message: " + e.ToString()); }
        }

        public static byte[] ReadImage(string filename, string currentchat, int dim)
        {
            byte[] bytes = new byte[dim];
            try
            {
                if (File.Exists(Path.Combine(path_chats, currentchat, "imgs", filename))) 
                {
                    FileStream fs = new FileStream(Path.Combine(path_chats, currentchat, "imgs", filename), FileMode.Open);
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Close();
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            return bytes;
        }

        public static void SaveImage(string chat, string filename, int filesize, byte[] img_bytes)
        {
            try
            {
                string path = Path.Combine(path_chats, chat, "imgs", filename);
                FileStream fs = new FileStream(path, FileMode.Create);
                fs.Write(img_bytes, 0, filesize);
                fs.Close();
            }
            catch (Exception e) { Console.WriteLine("Errore nel salvataggio dell'immagine: " + e.ToString()); }
        }

        public static long GetFileDim(string filename, string currentchat)
        {
            return new System.IO.FileInfo(Path.Combine(path_chats, currentchat, "imgs", filename)).Length;
        }
    }
}
