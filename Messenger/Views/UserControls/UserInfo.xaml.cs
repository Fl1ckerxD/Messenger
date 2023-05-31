using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Messenger.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для UserInfo.xaml
    /// </summary>
    public partial class UserInfo : UserControl
    {
        public UserInfo()
        {
            InitializeComponent();
            DataContext = new UserInfoViewModel { userInfo = this };
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
        private void Background_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close_Click(sender, e);
        }
    }
}
