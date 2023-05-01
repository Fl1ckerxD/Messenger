using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Classes
{
    internal class Settings
    {
        /// <summary>
        /// Сохранение логина и пароля для автоматической авторизации
        /// </summary>
        public static void RememberMe(string userName, string password)
        {
            Properties.Settings.Default.UserName = userName;
            Properties.Settings.Default.Password = password;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Удаление логина и пароля
        /// </summary>
        public static void NotRememberMe()
        {
            Properties.Settings.Default.UserName = "";
            Properties.Settings.Default.Password = "";
            Properties.Settings.Default.Save();
        }
    }
}
