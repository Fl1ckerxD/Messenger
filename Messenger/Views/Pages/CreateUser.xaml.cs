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
        private void showPassword_Checked(object sender, RoutedEventArgs e)
        {
            tb_Password.Text = pb_Password.Password;
            tb_Password.Visibility = Visibility.Visible;
            pb_Password.Visibility = Visibility.Collapsed;
        }

        private void showPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            pb_Password.Password = tb_Password.Text;
            tb_Password.Visibility = Visibility.Collapsed;
            pb_Password.Visibility = Visibility.Visible;
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            ViewModelManager.mainViewModel.OpenMainPage();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as CreateUserViewModel).User.Password = pb_Password.Password;
        }
    }
}
