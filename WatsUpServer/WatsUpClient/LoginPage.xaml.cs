using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

namespace WatsUpClient
{
    /// <summary>
    /// Logica di interazione per LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        char sep_char;
        public LoginPage()
        {
            InitializeComponent();
        }

        private void bt_login_Click(object sender, RoutedEventArgs e)
        {
            TcpClient connection = new TcpClient(Dns.GetHostName(), 45989);
            if (connection.Connected)
            {
                NetworkStream channel = connection.GetStream();
                if (tb_username.Text != "" && tb_username.Text != null)
                {
                    SendMessage(channel, "log" + sep_char + tb_username.Text);

                    string s = RecieveString(channel, 10);
                    if (s == "log_ok")
                    {
                        (Window.GetWindow(this) as MainWindow).connection = connection;
                        (Window.GetWindow(this) as MainWindow).StartPage();
                    }
                    else
                        MessageBox.Show("Registrazione non riuscita: " + s);
                }
            }
            //(Window.GetWindow(this) as MainWindow).connection;
        }

        private void bt_registration_Click(object sender, RoutedEventArgs e)
        {
            TcpClient connection = new TcpClient(Dns.GetHostName(), 45989);
            if (connection.Connected) {
                NetworkStream channel = connection.GetStream();
                if (tb_username.Text != "" && tb_username.Text != null)
                {
                    SendMessage(channel, "reg" + sep_char + tb_username.Text);

                    string s = RecieveString(channel, 10);
                    if (s == "reg_ok")
                        MessageBox.Show("Registrazione Completata");
                    else
                        MessageBox.Show("Registrazione non riuscita: " + s);
                }
            }
        }

        void SendMessage(NetworkStream channel, string msg)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(msg);
                channel.Write(buffer, 0, buffer.Length);
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        string RecieveString(NetworkStream channel, int buffer_dim)
        {
            byte[] bytes = new byte[buffer_dim];
            int n = channel.Read(bytes, 0, bytes.Length);
            return Encoding.ASCII.GetString(bytes, 0, n);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if((Window.GetWindow(this) as MainWindow) != null)
                sep_char = (Window.GetWindow(this) as MainWindow).sep_char;
        }
    }
}
