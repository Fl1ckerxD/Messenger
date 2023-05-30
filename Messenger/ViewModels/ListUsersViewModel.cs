using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Messenger.ViewModels
{
    class ListUsersViewModel : ListViewModel
    {
        private MessengerContext _context;
        private ObservableCollection<User> _chachedEmployees;
        private ObservableCollection<User> _employees;
        public ICommand Edit { get; }
        public ICommand Delete { get; }
        public ICommand CreateUserCommand { get; }
        public ObservableCollection<User> Employees
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

        private void RefreshDB()
        {
            Employees = new ObservableCollection<User>(_context.Users.Include(x => x.UserType));
            Employees.Remove(_context.Users.Where(x => x.Id == 20).FirstOrDefault());
        }

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
        protected override void Search(string search)
        {
            Employees = new ObservableCollection<User>(_chachedEmployees.Where(x => x.LastName.Contains(search, StringComparison.OrdinalIgnoreCase)
            || x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
