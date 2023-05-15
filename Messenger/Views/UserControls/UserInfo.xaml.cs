using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Messenger.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для UserInfo.xaml
    /// </summary>
    public partial class UserInfo : UserControl
    {
        //public User SelectedUser { get; set; }
        public UserInfo()
        {
            InitializeComponent();
            DataContext = new UserInfoViewModel { userInfo = this };
            //(DataContext as UserInfoViewModel).SelectedUser = SelectedUser;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            //(DataContext as UserInfoViewModel).SelectedUser = null;
            Visibility = Visibility.Collapsed;
        }

        private void Background_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close_Click(sender, e);
        }

        //private void UserControl_LayoutUpdated(object sender, EventArgs e)
        //{
        //    if((DataContext as UserInfoViewModel).SelectedUser != null)
        //        Visibility = Visibility.Visible;
        //}
    }
}
