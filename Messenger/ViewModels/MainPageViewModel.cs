using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace Messenger.ViewModels
{
    internal class MainPageViewModel : ViewModelBase
    {
        private MessengerContext _context; //Контекст базы данных
        public User CurrentUser { get; set; } //Текущий пользователь
        public ICommand ShowProfileCommand { get; } //Команда открытия редактирования профиля
        private ObservableCollection<User> _chachedEmployees; //Кэшированый список сотрудников
        private ObservableCollection<User> _employees;
        public ObservableCollection<User> Employees //Список сотрудников
        {
            get => _employees;
            set { _employees = value; OnPropertyChanged(); }
        }
        private string _textToFilter;
        public string TextToFilter //Текст введенный пользователем
        {
            get { return _textToFilter; }
            set { _textToFilter = value; OnPropertyChanged(); Search(value); }
        }
        public MainPageViewModel()
        {
            _context = new MessengerContext();
            CurrentUser = LoggedUser.currentUser;
            ShowProfileCommand = new RelayCommand(obj => { ViewModelManager.UserInfoControl.CurrentUser = (obj as User); });
            Employees = new ObservableCollection<User>(_context.Users.Where(x => x.DepartmentId == CurrentUser.DepartmentId).Include(x => x.Post));
            _chachedEmployees = Employees;
        }
        /// <summary>
        /// Поиск сотрудников по названию
        /// </summary>
        /// <param name="search">Строка с текстов для поиска</param>
        private void Search(string search)
        {
            Employees = new ObservableCollection<User>(_chachedEmployees.Where(x => x.LastName.Contains(search, StringComparison.OrdinalIgnoreCase)
            || x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
