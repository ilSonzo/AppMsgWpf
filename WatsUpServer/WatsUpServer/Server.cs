using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace WatsUpServer
{
    internal class Server
    {
        TcpClient connection;
        NetworkStream channel;
        char sep_char = ':';
        User logged_user;
        string current_chat;
        //Queue<Message> new_messages;

        public Server(TcpClient c)
        {
            connection = c;
            channel = connection.GetStream();
            //new_messages = new Queue<Message>();
        }

        public void Start()
        {
            bool exit = false;

            string msg_i = RecieveString(1024);

            string command_log = msg_i.Split(sep_char)[0];
            string payload_log = msg_i.Split(sep_char)[1];

            if(command_log == "log")
            {
                string r = (UserLog(payload_log)) ? "log_ok" : "log_no";
                    
                SendString(r);

                foreach(User user in GestioneFile.ReadUsers())
                {
                    if(user.Username.Equals(payload_log))
                        logged_user = user;
                }
            }
            else if(command_log == "reg")
            {
                string r = (CreateUser(payload_log)) ? "reg_ok" : "reg_no";
                    
                SendString(r);
                exit = true;
            }

            while (!exit)
            {
                string msg = RecieveString(1024);

                string[] splitted_msg = msg.Split(sep_char);
                string command = splitted_msg[0];
                string payload = "";
                if(splitted_msg.Length > 1)
                    payload = splitted_msg[1];

                switch (command)
                {
                    case "usr":
                        SendString(logged_user.Username);
                        break;
                    case "nch":                 //crea una nuova chat                                                                       nch:username
                        NewChat(payload);
                        break;
                    case "rcs":                 //richiede l'elenco delle chat di un utente                                                 rcs
                        GetChatsNames();
                        break;
                        /*
                    case "rcn":
                        string nmess = SerializeChat(new_messages.ToList());
                        new_messages.Clear();
                        SendString(nmess.Length + "");
                        RecieveString(10);
                        SendString(nmess);
                        break;
                        */
                    case "rch":                 //richiede contenuto di una chat specifica                                                  rch:chat
                        GetChat(payload);
                        break;
                    case "msg":                 //manda un messaggio da registrare su file                                                  msg:messaggioinxml
                        int n;
                        if (int.TryParse(payload, out n))
                        {
                            SendString("ok");
                            string str = RecieveString(n);
                            SendMessage(current_chat, str);
                        }
                        else
                            SendString("no");
                        break;
                    case "set":                 //imposta la chat su cui si sta comunicando (da chiamare ogni volta che si cambia chat)     set:chat
                        current_chat = payload;
                        //new_messages.Clear();
                        break;
                    case "smg":                 //salva una specifica immagine                                                              smg:nomefile.ext/dim --> smg:nomefile.ext
                        //SaveImage(payload.Split('/')[0], int.Parse(payload.Split('/')[1]));
                        SaveImage(payload);
                        break;
                    case "img":                 //richiede una specifica immagine                                                           img:nomefile.ext
                        SendImage(payload);
                        break;
                    case "ext":                 //esce
                        exit = true;
                        break;
                }
            }
            channel.Close();
            connection.Close();
        }

        bool CreateUser(string username)
        {
            bool user_exists = false;
            List<User> users = GestioneFile.ReadUsers();
            foreach (User user in users)
            {
                if(user.Username.Equals(username))
                    user_exists = true;
            }
            if (user_exists)
            {
                return false;
            }
            else
            {
                users.Add(new User(username));
                GestioneFile.SaveUsers(users);
                return true;
            }
        }

        bool UserLog(string username)
        {
            bool logged = false;
            List<User> users = GestioneFile.ReadUsers();
            for(int i = 0; i < users.Count && !logged; i++)
            {
                if (users[i].Equals(new User(username)))
                    logged = true;
            }
            return logged;
        }

        void NewChat(string username)
        {
            List<User> users = GestioneFile.ReadUsers();
            bool found1 = false;
            int pos1 = 0;
            bool found2 = false;
            int pos2 = 0;
            for(int i = 0; i < users.Count && !(found1 && found2);i++)
            {
                if (users[i].Equals(logged_user))
                {
                    pos1 = i;
                    found1 = true;
                }
                else if (users[i].Username.Equals(username))
                {
                    pos2 = i;
                    found2 = true;
                }
            }

            if(found1 && found2)
            {
                string chat_name = GestioneFile.CreateNewChat(logged_user.Username, username);
                users[pos1].Chats.Add(chat_name);
                logged_user = users[pos1];
                users[pos2].Chats.Add(chat_name);
                GestioneFile.SaveUsers(users);
                SendString("ok");
            }
            else
            {
                SendString("no");
            }
        }

        void GetChatsNames()
        {
            string str = string.Join(sep_char.ToString(), logged_user.Chats);
            SendString(str.Length + "");
            string response = RecieveString(10);
            if(response == "ok")
                SendString(str);
        }

        void GetChat(string chat_name)
        {
            List<Message> chat = GestioneFile.GetUserChat(chat_name);
            string str = SerializeChat(chat);
            SendString(str.Length+"");
            string response = RecieveString(10);
            if (response == "ok")
                SendString(str);
        }

        void SendMessage(string chat, string payload)
        {
            Message message = DeserializeMessage(payload);
            //new_messages.Enqueue(message);
            GestioneFile.AddMessageToChat(chat,message);
            SendString("ok");
        }

        void SendString(string s)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(s);
            channel.Write(buffer, 0, buffer.Length);
        }

        Message DeserializeMessage(string str)
        {
            Message msg = null;
            try
            {
                StringReader sw = new StringReader(str);
                XmlSerializer xmls = new XmlSerializer(typeof(Message));
                msg = (Message)xmls.Deserialize(sw);
            }
            catch (Exception e) { Console.WriteLine("Errore con deserializzazione messaggio inviato: " + e.ToString()); }
            return msg;
        }

        string SerializeChat(List<Message> messages)
        {
            string str = "";
            try
            {
                StringWriter strw = new StringWriter();
                XmlSerializer xmls = new XmlSerializer(typeof(List<Message>));
                xmls.Serialize(strw, messages);
                str = strw.ToString();
            }
            catch (Exception e) { Console.WriteLine("Errore con serializzazione chat: " + e.ToString()); }
            return str;
        }

        string RecieveString(int buffer_dim)
        {
            try
            {
                byte[] bytes = new byte[buffer_dim];
                int n = channel.Read(bytes, 0, bytes.Length);
                return Encoding.ASCII.GetString(bytes, 0, n);
            }
            catch (Exception e) { Console.WriteLine("Errore lettura da Tcp: " + e.ToString()); }
            return "";
        }

        void SaveImage(string payload)
        {
            string filename = payload.Split('/')[0];
            string dim = payload.Split('/')[1];
            int buffer_dim;
            if (int.TryParse(dim, out buffer_dim))
            {
                SendString("ok");
                try
                {
                    byte[] bytes = new byte[buffer_dim];
                    int n = channel.Read(bytes, 0, bytes.Length);
                    GestioneFile.SaveImage(current_chat, filename, n, bytes);
                }
                catch (Exception e) { Console.WriteLine("Errore scrittura da Tcp: " + e.ToString()); }
            }
        }

        void SendImage(string filename)
        {
            int length = (int)GestioneFile.GetFileDim(filename, current_chat);
            SendString(length + "");
            string str = RecieveString(10);
            if(str == "ok")
            {
                byte[] bite = GestioneFile.ReadImage(filename, current_chat, length);
                channel.Write(bite, 0, bite.Length);
            }
        }
    }
}
