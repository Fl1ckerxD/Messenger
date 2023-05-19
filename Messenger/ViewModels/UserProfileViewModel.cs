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
        public ICommand SaveCommand { get; }
        public ICommand QuitCommand { get; }
        public string imagePath;
        public User CurrentUser { get; set; }

        public UserProfileViewModel()
        {
            _context = new MessengerContext();
            CurrentUser = _context.Users.Where(x => x.Id == LoggedUser.currentUser.Id).Include(x => x.Post).First();
            SaveCommand = new RelayCommand(ExecuteSaveCommand);
            QuitCommand = new RelayCommand(ExecuteQuitCommand);
        }

        private void ExecuteQuitCommand(object obj)
        {
            Settings.ForgetMe();
            LoggedUser.currentUser = null;
            FrameManager.mainFrame.Navigate(new Views.Pages.Authorization());
        }

        private void ExecuteSaveCommand(object obj)
        {
            try
            {
                CheckDemands();
                _context.Users.Update(CurrentUser);
                _context.SaveChanges();
                LoggedUser.currentUser = CurrentUser;
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
            if (string.IsNullOrWhiteSpace(CurrentUser.Password))
                throw new Exception("Пароль не введен");
            if (string.IsNullOrWhiteSpace(CurrentUser.Login))
                throw new Exception("Логин не введен");
            if (!string.IsNullOrWhiteSpace(CurrentUser.Email))
                if (!Email.IsValidEmail(CurrentUser.Email))
                    throw new Exception("Не правильная почта");
        }
    }
}

