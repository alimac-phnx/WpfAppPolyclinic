using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WpfApp
{
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            ComboBoxInitialize();
        }

        private void ComboBoxInitialize()
        {
            string[] parameters = { "-", "по фамилии", "по имени", "по возрасту", "по адресу" };
            comboBox.SelectedIndex = 0;

            foreach (string item in parameters)
            {
                comboBox.Items.Add(item);
            }
        }
        
        private void Search(int parameter)
        {
            UserInfo searchuser = new UserInfo();
            TCPClient client = new TCPClient();

            string filename = client.ConnectTo("get");
            var users = DataFile.DataFileContent(filename);
            searchuser = users.Where(b => b.Surname == surnameTXT.Text && b.Name == nameTXT.Text).FirstOrDefault();

            if (searchuser != null)
            {
                (List<UserInfo>, int) sames = SearchForSameUsers(searchuser, users);
                List<UserInfo> searchuserS = sames.Item1;
                int countOfSameUsers = sames.Item2;

                if (countOfSameUsers > 1) NavigationService.Navigate(new Page22(parameter, searchuserS));
                else NavigationService.Navigate(new Page21(searchuser));
            }
            else MessageBox.Show("Пациент не найден");
        }

        private (List<UserInfo>, int) SearchForSameUsers(UserInfo searchuser, List<UserInfo> users)
        {
            List<UserInfo> sameuserS = new List<UserInfo>();
            int counter = 0;

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Surname == searchuser.Surname && users[i].Name == searchuser.Name)
                {
                    counter++;
                    sameuserS.Add(users[i]);
                }
            }

            return (sameuserS, counter);
        }

        private void SearchPatbut_Click(object sender, RoutedEventArgs e)
        {
            int parameter = comboBox.SelectedIndex;
            Search(parameter);
        }

        private void AddPatbut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page0());
        }

        private void FindPatbut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }

        private void Label_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page0());
        }
    }
}
