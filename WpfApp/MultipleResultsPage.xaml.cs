using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WpfApp
{
    public partial class MultipleResultsPage : Page
    {
        public TCPClient Client { get; set; }

        public MultipleResultsPage(int parameter, List<UserInfo> searchuserS, TCPClient client)
        {
            Client = client;
            InitializeComponent();
            OrderSameUsers(parameter, searchuserS);
        }

        private void ToDataGrid(IOrderedEnumerable<UserInfo> userInfoSorted)
        {
            foreach (UserInfo item in userInfoSorted) dataGridXAML.Items.Add(item);
        }

        private void OrderSameUsers(int parameter, List<UserInfo> searchuserS)
        {
            switch (parameter)
            {
                case 0:
                    foreach (UserInfo item in searchuserS) dataGridXAML.Items.Add(item);
                    break;
                case 1:
                    IOrderedEnumerable<UserInfo> userInfoSorted = from p in searchuserS
                                     orderby p.Name
                                     select p;
                    ToDataGrid(userInfoSorted);
                    break;
                case 2:
                    userInfoSorted = from p in searchuserS
                                     orderby p.Surname
                                     select p;
                    ToDataGrid(userInfoSorted);
                    break;
                case 3:
                    userInfoSorted = from p in searchuserS
                                     orderby p.Birthday
                                     select p;
                    ToDataGrid(userInfoSorted);
                    break;
                case 4:
                    userInfoSorted = from p in searchuserS
                                     orderby p.Adress
                                     select p;
                    ToDataGrid(userInfoSorted);
                    break;
                default:
                    break;
            }
        }
        private void ToPatient_Click(object sender, RoutedEventArgs e)
        {
            List<UserInfo> users = DataFile.DataFileContent("p.json");
            UserInfo a = (UserInfo)dataGridXAML.SelectedItems[0];
            UserInfo user = users.Where(b => b.Surname == a.Surname && b.Birthday == a.Birthday).FirstOrDefault();

            NavigationService.Navigate(new PatientCardPage(user, Client));
        }

        private void AddPatbut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage(Client));
        }

        private void FindPatbut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SearchPage(Client));
        }

        private void Label_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage(Client));
        }
    }
}
