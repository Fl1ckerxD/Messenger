using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger.Classes
{
    internal static class LoggedUser
    {
        public static string userName;
        public static int userId;
        public static LoggedUserType userType;

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
