using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp
{
    interface IPage
    {
        void ErrorMessage(string error);
        void ClearFields(DependencyObject obj);
        void PrintUsers(List<UserInfo> users);
    }
}
