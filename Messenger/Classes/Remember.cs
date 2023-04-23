using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Classes
{
    internal class Remember
    {
        public static void RememberMe(string userName, string password)
        {
            Properties.Settings.Default.UserName = userName;
            Properties.Settings.Default.Password = password;
            Properties.Settings.Default.Save();
        }
        public static void NotRememberMe()
        {
            Properties.Settings.Default.UserName = "";
            Properties.Settings.Default.Password = "";
            Properties.Settings.Default.Save();
        }
    }
}
