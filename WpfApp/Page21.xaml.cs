using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Runtime.Serialization.Json;

namespace WpfApp
{
    public partial class Page21 : Page, IPage
    {
        private UserInfo userInfo;

        public Page21(UserInfo userInfo)
        {
            InitializeComponent();
            PassUserValues(userInfo);
        }

        private void PassUserValues(UserInfo userInfo)
        {
            this.userInfo = userInfo;
            surnameTXT.Text = userInfo.Surname;
            nameTXT.Text = userInfo.Name;
            secnameTXT.Text = userInfo.Secname;
            birthdayTXT.Text = userInfo.Birthday;
            adressTXT.Text = userInfo.Adress;
            phonenumberTXT.Text = userInfo.Phonenumber;
            workPlaceTXT.Text = userInfo.WorkPlace;
            workPositionTXT.Text = userInfo.WorkPosition;
            eMailTXT.Text = userInfo.EMail;

            if (userInfo.Pol == "Мужской") Male.IsChecked = true;
            else Female.IsChecked = true;
        }

        private void AddPatbut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page0());
        }

        private void FindPatbut_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
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

        private void ChangeEditingAccess(DependencyObject obj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                if (obj is TextBox box) box.IsReadOnly = !box.IsReadOnly;

                if (obj is RadioButton button) button.IsEnabled = button.IsEnabled == false;

                if (obj is DatePicker picker)  picker.IsEnabled = picker.IsEnabled ? true : true;

                ChangeEditingAccess(VisualTreeHelper.GetChild(obj, i));
            }
        }

        private void ChangeButtonEditingAccess(Button buttToShow, Button buttToHide)
        {
            buttToShow.Visibility = Visibility.Visible;
            buttToHide.Visibility = Visibility.Hidden;
        }

        private void ChangeButtonEditingAccess(Button buttToHide, Button buttToHide1, Button buttToHide2)
        {
            buttToHide.Visibility = Visibility.Hidden;
            buttToHide1.Visibility = Visibility.Hidden;
            buttToHide2.Visibility = Visibility.Hidden;
        }

        private void EditPatbut_Click(object sender, RoutedEventArgs e)
        {
            ChangeEditingAccess(this);
            ChangeButtonEditingAccess(SaveChangesPatbut, EditPatbut);
        }

        public void ErrorMessage(string disp)
        {
            string caption = "Error!";
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBox.Show(disp, caption, button, icon);
            PassUserValues(userInfo);
        }

        private void TryToSave()
        {
            UserInfo editUser = new UserInfo(surnameTXT.Text, nameTXT.Text, secnameTXT.Text, birthdayTXT.Text, adressTXT.Text, phonenumberTXT.Text,
                userInfo.Pol, workPlaceTXT.Text, workPositionTXT.Text, eMailTXT.Text);
            SingleSerialization(editUser);

            TCPClient client = new TCPClient();
            string result = client.ConnectTo("send to save");

            if (result != string.Empty) ErrorMessage(result);
            else
            {
                MessageBox.Show("Изменения сохранены");
                ClearFields(this);
            }
        }

        private void SaveChangesPatbut_Click(object sender, RoutedEventArgs e)
        {
            TryToSave();
            ChangeEditingAccess(this);
            ChangeButtonEditingAccess(EditPatbut, SaveChangesPatbut);
        }

        private MessageBoxResult DeleteWarningMessage()
        {
            string disp = "Вы уверены?";
            string caption = "Deleting";
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxButton button = MessageBoxButton.YesNo;
            return MessageBox.Show(disp, caption, button, icon);
        }

        public static void SingleSerialization(UserInfo users)
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

        private void Delete()
        {
            MessageBoxResult result = DeleteWarningMessage();

            if (result == MessageBoxResult.Yes)
            {
                UserInfo userInfoToDelete = new UserInfo(surnameTXT.Text, nameTXT.Text, secnameTXT.Text, birthdayTXT.Text, adressTXT.Text, phonenumberTXT.Text,
                (bool)Male.IsChecked ? "Мужской" : "Женский", workPlaceTXT.Text, workPositionTXT.Text, eMailTXT.Text);
                SingleSerialization(userInfoToDelete);

                TCPClient client = new TCPClient();

                client.ConnectTo("send to delete");
                MessageBox.Show("Пациент удалён");

                ClearFields(this);
                ChangeButtonEditingAccess(DeletePatbut, SaveChangesPatbut, EditPatbut);
            }
        }

        private void ShowMoreInfo()
        {
            toHide.Visibility = Visibility.Visible;
            toHide1.Visibility = Visibility.Visible;
            toHide2.Visibility = Visibility.Visible;
            workPlaceTXT.Visibility = Visibility.Visible;
            workPositionTXT.Visibility = Visibility.Visible;
            eMailTXT.Visibility = Visibility.Visible;
        }
        private void HideMoreInfo()
        {
            toHide.Visibility = Visibility.Hidden;
            toHide1.Visibility = Visibility.Hidden;
            toHide2.Visibility = Visibility.Hidden;
            workPlaceTXT.Visibility = Visibility.Hidden;
            workPositionTXT.Visibility = Visibility.Hidden;
            eMailTXT.Visibility = Visibility.Hidden;
        }

        private void DeletePatbut_Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }

        private void ShowMore_Click(object sender, RoutedEventArgs e)
        {
            if (toHide.IsVisible == false || workPlaceTXT.IsVisible == false) ShowMoreInfo();
            else HideMoreInfo();
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
