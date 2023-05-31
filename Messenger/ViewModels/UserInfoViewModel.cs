using Messenger.Views.UserControls;
using Microsoft.EntityFrameworkCore;

namespace Messenger.ViewModels
{
    internal class UserInfoViewModel : ViewModelBase
    {
        public UserInfo userInfo; //Страница
        private MessengerContext _context; //Контекст базы данных
        private User _currentUser;
        public User CurrentUser //Выбранный пользователь
        {
            get { return _currentUser; }
            set { _currentUser = User(value); OnPropertyChanged(); userInfo.Visibility = System.Windows.Visibility.Visible; }
        }
        public UserInfoViewModel()
        {
            _context = new MessengerContext();
            ViewModelManager.UserInfoControl = this;
        }
        /// <summary>
        /// Получение данных о сотруднике
        /// </summary>
        /// <returns>Возвращает пользователя из базы данных</returns>
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
