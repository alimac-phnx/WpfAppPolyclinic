using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Runtime.Serialization.Json;
using System.IO;

namespace WpfApp
{
    public partial class RegistrationPage : Page, IPage
    {
        public TCPClient Client { get; set; }
        public static List<UserInfo> usersDemo = new List<UserInfo>();

        public RegistrationPage(TCPClient client)
        {
            Client = client;
            InitializeComponent();
            if (!Client.connectFlag)
            {
                MessageBox.Show("Вы работаете в автономном режиме: внимательно проверьте введённые вами данные," +
                    " иначе они будут безвозвратно утрачены.");
            }
        }
        
        public static void Serialization(UserInfo user)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(UserInfo));
            MemoryStream memoryStream = new MemoryStream();

            jsonSerializer.WriteObject(memoryStream, user);
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader reader = new StreamReader(memoryStream);
            string json = reader.ReadToEnd();

            memoryStream.Close();
            reader.Close();

            File.WriteAllText(@"DEMO_p.json", json);
        }

        public void ClearFields(DependencyObject obj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {

                if (obj is TextBox box)
                {
                    box.Text = null;
                }

                if (obj is RadioButton button)
                {
                    button.IsChecked = false;
                }

                ClearFields(VisualTreeHelper.GetChild(obj, i));
            }
        }

        public void ErrorMessage(string disp)
        {
            string caption = "Error!";

            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxButton button = MessageBoxButton.OK;

            MessageBox.Show(disp, caption, button, icon);
        }

        private void TryToRegister()
        {
            UserInfo userInfo = new UserInfo(surnameTXT.Text, nameTXT.Text, secnameTXT.Text, birthdayTXT.Text, adressTXT.Text, phonenumberTXT.Text,
                (bool)Male.IsChecked ? "Мужской" : "Женский", workPlaceTXT.Text, workPositionTXT.Text, eMailTXT.Text);
            if (Client.connectFlag) Serialization(userInfo);
            else
            {
                if (string.IsNullOrWhiteSpace(surnameTXT.Text) || string.IsNullOrWhiteSpace(nameTXT.Text) || string.IsNullOrWhiteSpace(secnameTXT.Text) ||
                    string.IsNullOrWhiteSpace(birthdayTXT.Text) || string.IsNullOrWhiteSpace(adressTXT.Text) || string.IsNullOrWhiteSpace(phonenumberTXT.Text) ||
                    ((bool)Male.IsChecked != true && (bool)Female.IsChecked != true)) { ErrorMessage("Основные поля должны быть заполнены.\n"); }
                else
                {
                    usersDemo.Add(userInfo);
                }
            }
            string result = Client.ConnectTo("send to register");

            if (result != string.Empty && result != "rejected") ErrorMessage(result);
            else
            {
                Client.changesFlag = true;
                MessageBox.Show("Пациент зарегистрирован.");

                ClearFields(this);
            }
        }

        private void CreatePatbut_Click(object sender, RoutedEventArgs e)
        {
            TryToRegister();
        }

        private void FindPatbut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SearchPage(Client));
        }

        private void Label_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage(Client));
        }

        public void PrintUsers(List<UserInfo> users)
        {
        }
    }
}
