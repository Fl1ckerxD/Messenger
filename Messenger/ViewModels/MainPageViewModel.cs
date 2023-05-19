using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace Messenger.ViewModels
{
    internal class MainPageViewModel : ViewModelBase
    {
        private MessengerContext _context;
        public User CurrentUser { get; set; }
        public ICommand ShowProfileCommand { get; }
        //public ICommand OpenEmployeesListCommand { get; }
        //public ICommand OpenChatsListCommand { get; }
        private ObservableCollection<User> _chachedEmployees;
        //private ObservableCollection<User> _chachedChats;

        private ObservableCollection<User> _employees;
        public ObservableCollection<User> Employees
        {
            get => _employees;
            set { _employees = value; OnPropertyChanged(); }
        }

        private string _textToFilter;
        public string TextToFilter
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

            //OpenEmployeesListCommand = new RelayCommand(ExecuteOpenEmployeesListCommand);
            //OpenChatsListCommand = new RelayCommand(ExecuteOpenChatsListCommand);

        }

        private void Search(string search)
        {
            Employees = new ObservableCollection<User>(_chachedEmployees.Where(x => x.LastName.Contains(search, StringComparison.OrdinalIgnoreCase)
            || x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
