using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Messenger.ViewModels
{
    class ListUsersViewModel : ListViewModel
    {
        private MessengerContext _context; //Контекст базы данных
        private ObservableCollection<User> _chachedEmployees; //Кэшированый список сотрудников
        private ObservableCollection<User> _employees;
        public ICommand Edit { get; } //Команда редактирования
        public ICommand Delete { get; } //Команда удаления
        public ICommand CreateUserCommand { get; } //Команда создания пользователя
        public ObservableCollection<User> Employees //Список сотрудников
        {
            get => _employees;
            set { _employees = value; OnPropertyChanged(); }
        }
        public ListUsersViewModel()
        {
            _context = new MessengerContext();
            RefreshDB();
            _chachedEmployees = Employees;
            Edit = new RelayCommand(obj => { ViewModelManager.mainViewModel.CurrentChildView = new CreateUserViewModel(obj as User, true); });
            Delete = new RelayCommand(ExecuteDelete);
            CreateUserCommand = new RelayCommand(obj => { ViewModelManager.mainViewModel.CurrentChildView = new CreateUserViewModel(null); });
        }
        /// <summary>
        /// Обновление списка сотрудников
        /// </summary>
        private void RefreshDB()
        {
            Employees = new ObservableCollection<User>(_context.Users.Include(x => x.UserType));
            Employees.Remove(_context.Users.Where(x => x.Id == 20).FirstOrDefault());
        }
        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="obj">Объект типа Model.User</param>
        private void ExecuteDelete(object obj)
        {
            User user = obj as User;
            var Result = MessageBox.Show(string.Format("Удалить пользователя {0} {1} {2}?", user.LastName, user.Name, user.Patronymic), "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                _context.Entry(user).State = EntityState.Deleted;
                _context.Users.Remove(user);
                _context.SaveChanges();
                RefreshDB();
            }
        }
        /// <summary>
        /// Поиск сотрудников по ФИО
        /// </summary>
        /// <param name="search">Строка с текстов для поиска</param>
        protected override void Search(string search)
        {
            Employees = new ObservableCollection<User>(_chachedEmployees.Where(x => x.LastName.Contains(search, StringComparison.OrdinalIgnoreCase)
            || x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
