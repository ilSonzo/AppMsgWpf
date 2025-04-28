using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace WatsUpClient
{
    /// <summary>
    /// Logica di interazione per MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        TcpClient connection;
        NetworkStream channel;
        char sep_char;
        string user_username;
        string current_chat;
        List<Message> current_messages;
        Mutex mutex;
        DispatcherTimer updater;
        public MainPage(TcpClient t, char sc)
        {
            InitializeComponent();
            connection = t;
            channel = connection.GetStream();
            sep_char = sc;
            mutex = new Mutex();
            updater = new DispatcherTimer();
        }

        private void bt_logout_Click(object sender, RoutedEventArgs e)
        {
            if(channel != null && connection != null)
            {
                mutex.WaitOne();
                SendString("ext");
                connection.Close();
                channel.Close();
                mutex.ReleaseMutex();
            }
            updater.Stop();
            (Window.GetWindow(this) as MainWindow).SetLogin();
        }

        private void bt_allega_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Immagini(*.jpg; *.jpeg; *.png; *.bmp; *.gif)| *.jpg; *.jpeg; *.png; *.bmp; *.gif";
            if ((bool)ofd.ShowDialog())
            {
                int dim = (int)GestioneFile.GetFileDim(ofd.FileName);
                mutex.WaitOne();
                SendString("smg" + sep_char + ofd.SafeFileName + "/" + dim);
                string resp = RecieveString(10);
                if (resp == "ok") {
                    byte[] b = GestioneFile.ReadImage(ofd.FileName, dim);
                    channel.Write(b,0,b.Length);
                }
                mutex.ReleaseMutex();
                Message msg = new Message("img", ofd.SafeFileName, DateTime.Now, user_username);
                SendMessage(msg);
            }

        }

        private void Send(object sender, RoutedEventArgs e)
        {
            if (current_chat != null && current_chat != "")
            {
                
                string str = tb_messaggio.Text;
                Message msg = new Message("txt", str, DateTime.Now, user_username);
                SendMessage(msg);
                tb_messaggio.Text = "";
            }
            /*
            int n;
            if (int.TryParse(payload, out n))
            {
                SendString("ok");
                string str = RecieveString(n);
                SendMessage(current_chat, str);
            }
            else
                SendString("no");
            */
        }

        void SendMessage(Message msg)
        {
            mutex.WaitOne();
            string msgstr = SerializeMessage(msg);
            SendString("msg" + sep_char + msgstr.Length);
            string s = RecieveString(10);
            if (s == "ok")
            {
                SendString(msgstr);
                string resp = RecieveString(10);
                if (resp != "ok")
                    MessageBox.Show("Megaerrore con sending");
                /*else
                {
                    lv_chat.ItemsSource = null;
                    current_messages.Add(msg);
                    lv_chat.ItemsSource = current_messages;
                }*/
            }
            mutex.ReleaseMutex();
        }

        //fatto
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mutex.WaitOne();
            SendString("rcs");
            string resp = RecieveString(1024);
            int n;
            if (int.TryParse(resp, out n))
            {
                if (n > 0)
                {
                    SendString("ok");
                    string response = RecieveString(n);
                    List<string> chats = response.Split(sep_char).ToList();
                    lv_chatslist.ItemsSource = chats;
                }
                else
                    SendString("no");
            }
            else
                SendString("no");

            SendString("usr");
            user_username = RecieveString(1024);
            mutex.ReleaseMutex();

            updater.Interval = TimeSpan.FromMilliseconds(500);
            updater.Tick += Ticker;
            updater.Start();

            lv_chat.Background = Brushes.Aqua;
            /*
            
            string str = string.Join(sep_char.ToString(), logged_user.Chats);
            SendString("len:"+str.Length);
            string response = RecieveString(10);
            if(response == "ok")
                SendString(str);
            */
        }

        private void Ticker(object sender, System.EventArgs e)
        {
 	        UpdateChat();
        }

        private void bt_createchat_Click(object sender, RoutedEventArgs e)
        {
            
            if (tb_creachat.Text != null && tb_creachat.Text != "")
            {
                mutex.WaitOne();
                SendString("nch" + sep_char + tb_creachat.Text);
                string resp = RecieveString(1024);
                mutex.ReleaseMutex();
                if (resp == "ok")
                {
                    MessageBox.Show("Chat creata con successo");
                }
                else
                {
                    MessageBox.Show("utente inserito non esistente");
                }
                tb_creachat.Text = "";
            }
            else
                MessageBox.Show("devi inserire il nome dell'utente con cui vuoi comunicare");
        }

        void SendString(string s)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(s);
                channel.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex) { MessageBox.Show("Errore Send:\n" + ex.Message); }
        }

        string RecieveString(int buffer_dim)
        {
            string str = "";
            try
            {
                byte[] bytes = new byte[buffer_dim];
                int n = channel.Read(bytes, 0, bytes.Length);
                str = Encoding.ASCII.GetString(bytes, 0, n);
            }
            catch (Exception ex) { MessageBox.Show("Errore ricezione:\n" + ex.Message); }
            return str;
        }

        string SerializeMessage(Message msg)
        {
            string str = "";
            try
            {
                StringWriter strw = new StringWriter();
                XmlSerializer xmls = new XmlSerializer(typeof(Message));
                xmls.Serialize(strw,msg);
                str = strw.ToString();
            }
            catch (Exception e) { MessageBox.Show("errore: " + e.Message); }
            return str;
        }
        List<Message> DeserializeChat(string str)
        {
            List<Message> msg = null;
            try
            {
                StringReader sw = new StringReader(str);
                XmlSerializer xmls = new XmlSerializer(typeof(List<Message>));
                msg = (List<Message>)xmls.Deserialize(sw);
            }
            catch (Exception e) { MessageBox.Show("errore: " + e.Message); }
            return msg;
        }

        void lv_chatslist_Selected(object sender, RoutedEventArgs e)
        {
            if (lv_chatslist.SelectedItem != null)
            {
                mutex.WaitOne();
                SendString("set" + sep_char + lv_chatslist.SelectedItem.ToString());
                current_chat = lv_chatslist.SelectedItem.ToString();

                SendString("rch" + sep_char + lv_chatslist.SelectedItem.ToString());
                int n;
                string ns = RecieveString(180);
                if (int.TryParse(ns, out n))
                {
                    SendString("ok");
                    string chat = RecieveString(n);
                    List<Message> messages = DeserializeChat(chat);
                    lv_chat.Items.Clear();
                    AddMessages(messages);
                }
                else
                    SendString("no");
                mutex.ReleaseMutex();
            }
        }

        void UpdateChat()
        {
            if(current_chat != null && current_chat != "")
            {
                /*
                mutex.WaitOne();
                SendString("rcn" + sep_char + lv_chatslist.SelectedItem.ToString());
                int n;
                string ns = RecieveString(180);
                if (int.TryParse(ns, out n))
                {
                    SendString("ok");
                    string chat = RecieveString(n);
                    List<Message> messages = DeserializeChat(chat);
                    current_messages.AddRange(messages);
                    lv_chat.ItemsSource = current_messages;
                }
                mutex.ReleaseMutex();
                */

                mutex.WaitOne();
                SendString("rch" + sep_char + lv_chatslist.SelectedItem.ToString());
                int n;
                string ns = RecieveString(180);
                if (int.TryParse(ns, out n))
                {
                    SendString("ok");
                    string chat = RecieveString(n);
                    List<Message> messages = DeserializeChat(chat);
                    lv_chat.Items.Clear();
                    AddMessages(messages);
                }
                else
                    SendString("no");
                mutex.ReleaseMutex();
            }
        }

        private void tb_creachat_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                bt_createchat_Click(sender, e);
        }

        private void tb_messaggio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Send(sender, e);
        }

        void AddMessages(List<Message> msg_list)
        {
            foreach(Message m in msg_list)
            {
                if(m.MsgType == "img")
                    if(!File.Exists(System.IO.Path.Combine("imgs",m.Content)))
                    {
                        mutex.WaitOne();
                        SendString("img" + sep_char + m.Content);
                        string strdim = RecieveString(180);
                        int n;
                        if (int.TryParse(strdim, out n))
                        {
                            SendString("ok");
                            byte[] b = new byte[n];
                            channel.Read(b, 0, n);
                            GestioneFile.SaveImage(m.Content, b.Length, b);
                        }
                        else
                            SendString("no");
                        mutex.ReleaseMutex();
                        /*
                        int length = (int)GestioneFile.GetFileDim(filename, current_chat);
                        SendString(length + "");
                        string str = RecieveString(10);
                        if (str == "ok")
                        {
                            byte[] bite = GestioneFile.ReadImage(filename, current_chat, length);
                            channel.Write(bite, 0, bite.Length);
                        }
                        */
                    }
                AddMessageToListBox(m);
            }
        }

        void AddMessageToListBox(Message message)
        {
            // Contenitore principale per il messaggio
            StackPanel stackPanel = new StackPanel { Orientation = Orientation.Vertical, Margin = new Thickness(5) };
            Border border = new Border() {
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(5),
                Margin = new Thickness(2),
            };
            if (message.Sender == user_username)
                border.Background = Brushes.DeepSkyBlue;
            else
                border.Background = Brushes.White;
            stackPanel.Children.Add(border);
            

            if (message.MsgType == "txt")
            {
                // Messaggio testuale
                TextBlock textBlock = new TextBlock
                {
                    Text = message.Content,
                    FontSize = 14,
                    Padding = new Thickness(5),
                    TextWrapping = TextWrapping.Wrap
                };
                border.Child = textBlock;
            }
            else if (message.MsgType == "img")
            {
                // Controllo immagine
                Image image = new Image
                {
                    Width = 300,
                    Height = 300,
                    Margin = new Thickness(5)
                };
                image.BeginInit();
                BitmapImage btm = new BitmapImage();
                btm.BeginInit();
                btm.UriSource = new Uri(System.IO.Path.Combine("imgs", message.Content), UriKind.RelativeOrAbsolute);
                btm.CacheOption = BitmapCacheOption.OnLoad;
                btm.EndInit();
                image.Source = btm;
                border.Child = image;
            }

            TextBlock footer = new TextBlock
            {
                Text = string.Format("{0:dd/MM/yyyy HH:mm}", message.DateOfMsg),
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                Foreground = System.Windows.Media.Brushes.Gray,
                Margin = new Thickness(0, 0, 0, 3)
            };
            stackPanel.Children.Add(footer);

            // Creazione dell'elemento ListBoxItem
            ListBoxItem item = new ListBoxItem { Content = stackPanel };

            // Aggiungere l'elemento alla ListBox
            lv_chat.Items.Add(item);
        }
    }
}
