using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace Messenger.ViewModels
{
    class UserProfileViewModel : ViewModelBase
    {
        private MessengerContext _context; //Контекст базы данных
        public string imagePath; //Путь к изображению
        public string CurrentPassword { get; set; } //Текущий пароль
        public string NewPassword { get; set; } //Новый пароль
        public string ConfirmPassword { get; set; } //Повторный новый пароль
        public Visibility AdminVisible { get; set; } //Видимость кнопки для администрации
        public ICommand SaveCommand { get; } //Команда сохранения изменений
        public ICommand QuitCommand { get; } //Команда выхода из учетной записи
        public ICommand BackCommand { get; } //Команда возвращающая на предыдущую страницу
        public ICommand OpenAdminPageCommand { get; } //Команда открытия окна администрации
        public User CurrentUser { get; set; } //Текущий авторизованный пользователь
        public UserProfileViewModel(User user)
        {
            _context = new MessengerContext();
            CurrentUser = user;
            SaveCommand = new RelayCommand(ExecuteSaveCommand);
            QuitCommand = new RelayCommand(ExecuteQuitCommand);
            BackCommand = new RelayCommand(obj => { ViewModelManager.mainViewModel.CurrentChildView = new MainPageViewModel(); });//FrameManager.mainFrame.GoBack();
            OpenAdminPageCommand = new RelayCommand(obj => { ViewModelManager.mainViewModel.CurrentChildView = new AdminPageViewModel(); });
            AdminVisible = LoggedUser.userType.GetHashCode() == 1 ? Visibility.Visible : Visibility.Collapsed;
        }
        /// <summary>
        /// Выход из учетной записи
        /// </summary>
        private void ExecuteQuitCommand(object obj)
        {
            var Result = MessageBox.Show("Выйти из учётной записи?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                Settings.ForgetMe();
                FrameManager.mainFrame.Navigate(new Views.Pages.Authorization());
            }
        }
        /// <summary>
        /// Сохранение изменений
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteSaveCommand(object obj)
        {
            try
            {
                CheckDemands();
                if (!string.IsNullOrWhiteSpace(CurrentPassword))
                    CurrentUser.Password = NewPassword;
                _context.Entry(CurrentUser).State = EntityState.Modified;
                _context.SaveChanges();
                LoggedUser.currentUser = CurrentUser;
                if (Settings.HasUser())
                    Settings.RememberMe(CurrentUser.Login, CurrentUser.Password);
                MessageBox.Show("Изменения сохранены", "Сохранено", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Проверка требований
        /// </summary>
        private void CheckDemands()
        {
            if (string.IsNullOrWhiteSpace(CurrentUser.Name))
                throw new Exception("Имя не введено");
            if (string.IsNullOrWhiteSpace(CurrentUser.LastName))
                throw new Exception("Фамилия не введена");
            if (!string.IsNullOrWhiteSpace(CurrentPassword))
                if (CurrentPassword != CurrentUser.Password)
                    throw new Exception("Не правильный текущий пароль");
                else if (string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmPassword))
                    throw new Exception("Пароль не введен");
                else if (NewPassword != ConfirmPassword)
                    throw new Exception("Пароли не совпадают");
            if (string.IsNullOrWhiteSpace(CurrentUser.Login))
                throw new Exception("Логин не введен");
            if (!string.IsNullOrWhiteSpace(CurrentUser.Email))
                if (!ValidProperty.IsValidEmail(CurrentUser.Email))
                    throw new Exception("Не правильная почта");
            if (!string.IsNullOrWhiteSpace(CurrentUser.Phone))
                if (!ValidProperty.IsValidPhone(CurrentUser.Phone))
                    throw new Exception("Не правильный номер телефона");
        }
    }
}