using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
namespace WpfApp
{
    public partial class Page0 : Page, IPage
    {
        public Page0()
        {
            InitializeComponent();

            TCPClient client = new TCPClient();

            string filename = client.ConnectTo("get");
            var users = DataFile.DataFileContent(filename);

            ComboBoxInitialize();
            PrintUsers(users);
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

        private void AddPatbut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1());
        }

        private void FindPatbut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
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
