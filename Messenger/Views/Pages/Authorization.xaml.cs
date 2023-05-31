using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Messenger.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        Classes.Authorization auth = new Classes.Authorization();
        public Authorization()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обработчик событий запускаемый после загрузки страницы
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (auth.AutoLogin())
                    ViewModelManager.mainViewModel.CurrentChildView = new MainPageViewModel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Авторизация в программе
        /// </summary>
        private void b_Enter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (auth.Login(tb_Login.Text, pb_Password.Password, ckb_Remember.IsChecked))
                    ViewModelManager.mainViewModel.CurrentChildView = new MainPageViewModel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
