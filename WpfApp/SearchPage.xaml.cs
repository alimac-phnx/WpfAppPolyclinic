using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WpfApp
{
    public partial class SearchPage : Page
    {
        public TCPClient Client { get; set; }

        public SearchPage(TCPClient client)
        {
            Client = client;
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

        public static void Serialization(string fileCashName, string fileAutonomeName)
        {
            var usersCash = DataFile.DataFileContent(fileCashName);
            var usersAutonome = DataFile.DataFileContent(fileAutonomeName);
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<UserInfo>));
            MemoryStream memoryStream = new MemoryStream();

            foreach(var item in usersAutonome)
            {
                usersCash.Add(item);
            }

            jsonSerializer.WriteObject(memoryStream, usersCash);
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new StreamReader(memoryStream);
            string json = reader.ReadToEnd();

            memoryStream.Close();
            reader.Close();

            File.WriteAllText(@"pAutonomeSearching.json", json);
        }

        private void Search(int parameter)
        {
            UserInfo searchuser = new UserInfo();
            string filename;
            if+t.connectFlag) { filename = Client.ConnectTo("get"); }
            else 
            {
                Serialization("pCash.json", "pAutonome.json");
                filename = "pAutonomeSearching.json";
            }
            var users = DataFile.DataFileContent(filename);
            searchuser = users.Where(b => b.Surname.ToLower() == surnameTXT.Text.ToLower() && 
            b.Name.ToLower() == nameTXT.Text.ToLower()).FirstOrDefault();

            if (searchuser != null)
            {
                (List<UserInfo>, int) sames = SearchForSameUsers(searchuser, users);
                List<UserInfo> searchuserS = sames.Item1;
                int countOfSameUsers = sames.Item2;

                if (countOfSameUsers > 1) NavigationService.Navigate(new MultipleResultsPage(parameter, searchuserS, Client));
                else NavigationService.Navigate(new PatientCardPage(searchuser, Client));
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
            NavigationService.Navigate(new RegistrationPage(Client));
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
