using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.ViewModels
{
    class ListUsersViewModel : ListViewModel
    {
        private MessengerContext _context;
        private ObservableCollection<User> _chachedEmployees;
        private ObservableCollection<User> _employees;
        public ObservableCollection<User> Employees
        {
            get => _employees;
            set { _employees = value; OnPropertyChanged(); }
        }
        public ListUsersViewModel()
        {
            _context = new MessengerContext();
            Employees = new ObservableCollection<User>(_context.Users.Include(x => x.UserType));
            _chachedEmployees = Employees;
        }
        protected override void Search(string search)
        {
            Employees = new ObservableCollection<User>(_chachedEmployees.Where(x => x.LastName.Contains(search, StringComparison.OrdinalIgnoreCase)
            || x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
