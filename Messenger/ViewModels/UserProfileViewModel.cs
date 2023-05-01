using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace Messenger.ViewModels
{
    class UserProfileViewModel : ViewModelBase
    {
        private MessengerContext _context;
        public ICommand SaveCommand { get; }
        public string imagePath;
        public User CurrentUser { get; set; }

        public UserProfileViewModel()
        {
            _context = new MessengerContext();
            CurrentUser = _context.Users.Where(x => x.Id == LoggedUser.currentUser.Id).Include(x => x.Post).First();
            SaveCommand = new RelayCommand(ExecuteSaveCommand);
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
                if (!IsValidEmail(CurrentUser.Email))
                    throw new Exception("Не правильная почта");
        }

        /// <summary>
        /// Определяет, является ли получаемый текст почтой
        /// </summary>
        /// <returns>Возвращает true если является почтой, иначе false</returns>
        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}

