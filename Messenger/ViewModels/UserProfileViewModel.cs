using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Messenger.Views.Pages;
using Microsoft.EntityFrameworkCore;

namespace Messenger.ViewModels
{
    class UserProfileViewModel : ViewModelBase
    {
        private MessengerContext _context;
        public string imagePath;
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public Visibility AdminVisible { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand QuitCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand OpenAdminPageCommand { get; }
        public User CurrentUser { get; set; }

        public UserProfileViewModel()
        {
            _context = new MessengerContext();
            CurrentUser = _context.Users.Where(x => x.Id == LoggedUser.currentUser.Id).Include(x => x.Post).First();
            SaveCommand = new RelayCommand(ExecuteSaveCommand);
            QuitCommand = new RelayCommand(ExecuteQuitCommand);
            BackCommand = new RelayCommand(obj => { FrameManager.mainFrame.GoBack(); });//ViewModelManager.mainViewModel.CurrentChildView = new MainPageViewModel();
            OpenAdminPageCommand = new RelayCommand(obj => { ViewModelManager.mainViewModel.CurrentChildView = new AdminPageViewModel(); });
            AdminVisible = LoggedUser.userType.GetHashCode() == 1 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ExecuteQuitCommand(object obj)
        {
            var Result = MessageBox.Show("Выйти из учётной записи?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                Settings.ForgetMe();
                LoggedUser.currentUser = null;
                FrameManager.mainFrame.Navigate(new Views.Pages.Authorization());
            }
        }

        private void ExecuteSaveCommand(object obj)
        {
            try
            {
                CheckDemands();
                CurrentUser.Password = NewPassword;
                _context.Users.Update(CurrentUser);
                _context.SaveChanges();
                LoggedUser.currentUser = CurrentUser;
                if (Settings.HasUser())
                    Settings.RememberMe(CurrentUser.Login, CurrentUser.Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckDemands()
        {
            if (string.IsNullOrWhiteSpace(CurrentUser.Name))
                throw new Exception("Имя не введено");
            if (string.IsNullOrWhiteSpace(CurrentUser.LastName))
                throw new Exception("Фамилия не введена");
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

