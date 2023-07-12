using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Runtime.Serialization.Json;
using System.IO;

namespace WpfApp
{
    public partial class Page1 : Page, IPage
    {
        public Page1()
        {
            InitializeComponent();
        }
        
        public static void Serialization(UserInfo users)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(UserInfo));
            MemoryStream memoryStream = new MemoryStream();

            jsonSerializer.WriteObject(memoryStream, users);
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
            Serialization(userInfo);

            TCPClient client = new TCPClient();
            string result = client.ConnectTo("send to register");

            if (result != string.Empty) ErrorMessage(result);
            else
            {
                MessageBox.Show("Пациент зарегистрирован");
                ClearFields(this);
            }
        }

        private void CreatePatbut_Click(object sender, RoutedEventArgs e)
        {
            TryToRegister();
        }

        private void FindPatbut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }

        private void Label_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page0());
        }

        public void PrintUsers(List<UserInfo> users)
        {
        }
    }
}
