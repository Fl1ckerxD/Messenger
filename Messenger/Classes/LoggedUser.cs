namespace Messenger.Classes
{
    internal static class LoggedUser
    {
        public static User currentUser; //Авторизованный пользователь
        public static LoggedUserType userType; //Тип авторизованного пользователя
        public static int chatId;
        /// <summary>
        /// Получение типа пользователя
        /// </summary>
        /// <param name="userTypeId">Id типа пользователя</param>
        public static void SetUserType(int userTypeId)
        {
            switch (userTypeId)
            {
                case 1:
                    userType = LoggedUserType.Admin;
                    break;
                case 2:
                    userType = LoggedUserType.Moderator;
                    break;
                case 3:
                    userType = LoggedUserType.User;
                    break;
            }
        }
    }
    internal enum LoggedUserType
    {
        User,
        Admin,
        Moderator
    }
}
