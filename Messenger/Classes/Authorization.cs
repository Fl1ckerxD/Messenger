using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger.Classes
{
    internal class Authorization
    {
        internal void AutoLogin(string login, string password, bool? remember)
        {
            try
            {
                if (Properties.Settings.Default.UserName != string.Empty)
                {
                    login = Properties.Settings.Default.UserName;
                    password = Properties.Settings.Default.Password;
                    remember = true;
                    Login(login, password);
                }
            }
            catch 
            {
                throw new Exception("Ошибка подключения");
            }
        }
        internal void Login(string login, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                    MessageBox.Show("Введите логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                //Authorizate(login, password, true);
            }
            catch
            {
                throw new Exception("Ошибка подключения");
                MessageBox.Show("Ошибка подключения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool? ContainsUser()
        {
            //Login? login = supportContext.Logins.Where(x => x.Login1 == tb_Login.Text &&
            //x.Password == pb_Password.Password)
            //    .Include(x => x.User)
            //    .FirstOrDefault();
            return null;
        }
        internal void Authorizate(string login, string password, bool? remember)
        {
            if (ContainsUser() is not null)
            {
                try
                {
                    if (remember == true)
                        Remember.RememberMe(login, password);
                    else
                        Remember.NotRememberMe();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //LoggedUser.userName = login.User.Name;
                //LoggedUser.userId = login.User.Id;
                //LoggedUser.SetUserType(login.UserTypeId);

                //ViewModelManager.mainViewModel.ShowMainSupportView();
            }
            else MessageBox.Show("Неправильный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
