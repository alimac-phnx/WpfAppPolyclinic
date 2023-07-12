using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
namespace WpfApp
{
    public partial class MainPage : Page, IPage
    {
        public TCPClient Client { get; set; }
        private static int i;
        public static List<TCPClient> clients = new List<TCPClient>();

        public MainPage(TCPClient client/*, List<TCPClient> clients*/)
        {
            Client = client;
            Client.Name = "me" + i.ToString();
            clients.Add(client);
            i++;
            InitializeComponent();

            string filename = client.ConnectTo("get");
            var users = DataFile.DataFileContent(filename);
            ComboBoxInitialize();
            PrintUsers(users);
            if (!Client.connectFlag)
            {
                var usersDemo = DataFile.DataFileContent("pAutonome.json");
                PrintUsers(usersDemo);
            }
            WriteCash(users);
            client.UpdatesCheck(clients);
        }

        private void ComboBoxInitialize()
        {
            string[] parameters = { "-", "мужской", "женский"};
            comBoBox.SelectedIndex = 0;

            foreach (string item in parameters)
            {
                comBoBox.Items.Add(item);
            }
        }

        private void ToDataGrid(List<UserInfo> users)
        {
            List<UserInfo> lst = Filter(users);

            foreach (UserInfo item in lst)
                dataGridXAML.Items.Add(item);
        }

        public void PrintUsers(List<UserInfo> users)
        {
            for (int i = 0; i < users.Count; i++)
            {
                dataGridXAML.Items.Add(users[i]);
            }
        }

        private List<UserInfo> Filter(List<UserInfo> users)
        {
            List<UserInfo> fUsers = new List<UserInfo>();
            string parameter = comBoBox.SelectedItem.ToString();

            if (parameter != "-")
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Pol.ToLower() == parameter)
                    {
                        fUsers.Add(users[i]);
                    }
                }
                return fUsers;
            }
            else { return users; }
        }

        public void WriteCash(List<UserInfo> users)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<UserInfo>));
            MemoryStream memoryStream = new MemoryStream();

            jsonSerializer.WriteObject(memoryStream, users);
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new StreamReader(memoryStream);

            string json = reader.ReadToEnd();

            memoryStream.Close();
            reader.Close();

            File.WriteAllText(@"pCash.json", json);
        }

        private void AddPatbut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage(Client));
        }

        private void FindPatbut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SearchPage(Client));
        }

        //void IPage.PrintUsers(List<UserInfo> users)
        //{
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<UserInfo> users = DataFile.DataFileContent("p.json");
            dataGridXAML.Items.Clear();
            ToDataGrid(users);
        }

        public void ClearFields(DependencyObject obj)
        {
        }
        public void ErrorMessage(string error)
        {
        }
    }
}
