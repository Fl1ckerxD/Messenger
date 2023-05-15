using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger.ViewModels
{
    internal class MainPageViewModel : ViewModelBase
    {
        public User CurrentUser { get; set; }

        public MainPageViewModel()
        {
            CurrentUser = LoggedUser.currentUser;
        }
    }
}
