using System;
using System.Linq;
using System.Windows;

namespace Messenger.Classes
{
    internal class Authorization
    {
        private MessengerContext _context = new MessengerContext();

        internal void AutoLogin()
        {
            string login, password;
            bool? remember;
            try
            {
                if (Properties.Settings.Default.UserName != string.Empty)
                {
                    login = Properties.Settings.Default.UserName;
                    password = Properties.Settings.Default.Password;
                    remember = true;
                    Login(login, password, remember);
                }
            }
            catch
            {
                throw new Exception("Ошибка подключения");
            }
        }
        internal void Login(string login, string password, bool? remember = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Введите логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                User? user = GetUser(login, password);
                if (user is not null)
                {
                    if (remember == true)
                        Remember.RememberMe(login, password);
                    else
                        Remember.NotRememberMe();

                    LoggedUser.currentUser = user;
                    LoggedUser.SetUserType(user.UserTypeId);
                }
                else MessageBox.Show("Неправильный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                throw new Exception("Ошибка подключения");
            }
        }
        private User? GetUser(string login, string password)
        {
            return _context.Users.Where(x => x.Login == login && x.Password == password).FirstOrDefault();
        }
    }
}
