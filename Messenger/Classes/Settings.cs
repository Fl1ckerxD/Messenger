namespace Messenger.Classes
{
    internal static class Settings
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
        public static void ForgetMe()
        {
            Properties.Settings.Default.UserName = "";
            Properties.Settings.Default.Password = "";
            Properties.Settings.Default.Save();
        }
        /// <summary>
        /// Проверяет существует ли сохраненный пользователь
        /// </summary>
        /// <returns>Возвращает true если существует, иначе false</returns>
        public static bool HasUser()
        {
            if(Properties.Settings.Default.UserName.Length > 0)
                return true;
            return false;
        }
    }
}
