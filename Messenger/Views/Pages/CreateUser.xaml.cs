using System.Windows;
using System.Windows.Controls;

namespace Messenger.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Page
    {
        public CreateUser()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Показать пароль
        /// </summary>
        private void showPassword_Checked(object sender, RoutedEventArgs e)
        {
            tb_Password.Text = pb_Password.Password;
            tb_Password.Visibility = Visibility.Visible;
            pb_Password.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Скрыть пароль
        /// </summary>
        private void showPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            pb_Password.Password = tb_Password.Text;
            tb_Password.Visibility = Visibility.Collapsed;
            pb_Password.Visibility = Visibility.Visible;
        }
        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as CreateUserViewModel).User.Password = pb_Password.Password;
        }
    }
}
