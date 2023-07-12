using System.Collections.Generic;
using System.Windows;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private static int i = 0;
        public static List<TCPClient> clients = new List<TCPClient>();

        public MainWindow()
        {
            InitializeComponent();
            TCPClient client = new TCPClient("me" /*+ i*/);
            //clients.Add(client);
            //i++;
            MainFrame.Content = new MainPage(client/*, clients*/);
        }
    }
}