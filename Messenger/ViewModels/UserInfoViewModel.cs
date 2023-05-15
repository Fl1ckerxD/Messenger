using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.Views.UserControls;
using Microsoft.EntityFrameworkCore;

namespace Messenger.ViewModels
{
    internal class UserInfoViewModel : ViewModelBase
    {
        public UserInfo userInfo;
        private MessengerContext _context;
        private User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = User(value); OnPropertyChanged(); userInfo.Visibility = System.Windows.Visibility.Visible; }
        }
        public UserInfoViewModel()
        {
            _context = new MessengerContext();
            ViewModelManager.UserInfoControl = this;
        }

        private User User(User value)
        {
            return _context.Users.Where(x => x.Id == value.Id)
                .Include(x => x.Department)
                .Include(x => x.Post)
                .Include(x => x.Status)
                .First();
        }
    }
}
