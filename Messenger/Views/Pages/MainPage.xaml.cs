using System.Windows.Controls;
using System.Windows.Input;

namespace Messenger.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void Settings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModelManager.mainViewModel.CurrentChildView = new UserProfileViewModel(LoggedUser.currentUser);
        }
    }
}
