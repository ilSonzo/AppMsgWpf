﻿using System.Net.Sockets;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TcpClient connection;
        public char sep_char = ':';
        public MainWindow()
        {
            InitializeComponent();
        }

        public void StartPage()
        {
            widonwo.Children.Clear();
            widonwo.Children.Add(new MainPage(connection, sep_char));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            widonwo.Children.Add(new LoginPage());
        }

        public void SetLogin()
        {
            widonwo.Children.Clear();
            widonwo.Children.Add(new LoginPage());
        }
    }
}