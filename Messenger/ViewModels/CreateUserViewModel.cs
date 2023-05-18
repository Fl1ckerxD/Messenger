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
    internal class CreateUserViewModel : ViewModelBase
    {
        private MessengerContext _context;
        #region Public members
        public User User { get; set; }
        public Chat SelectedChat { get; set; }
        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<UserType> UserTypes { get; set; }
        public ICommand CreateUserCommand { get; }
        public ICommand SelectedItemChangedCommand { get; }
        #endregion
        #region Properties
        private ObservableCollection<Post> _posts;
        public ObservableCollection<Post> Posts
        {
            get => _posts;
            set { _posts = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Chat> _chats;
        public ObservableCollection<Chat> Chats
        {
            get => _chats;
            set { _chats = value; OnPropertyChanged(); }
        }
        #endregion
        public CreateUserViewModel()
        {
            _context = new MessengerContext();
            User = new User();
            Departments = new ObservableCollection<Department>(_context.Departments);
            UserTypes = new ObservableCollection<UserType>(_context.UserTypes);
            CreateUserCommand = new RelayCommand(ExecuteCreateUserCommand);
            SelectedItemChangedCommand = new RelayCommand(ExecuteSelectedItemChangedCommand);
        }

        private void ExecuteSelectedItemChangedCommand(object obj)
        {
            Posts = new ObservableCollection<Post>(_context.Posts.Where(x => x.Departments.Contains((obj as Department))));
            Chats = new ObservableCollection<Chat>(_context.Chats.Where(x => x.DepartmentId == (obj as Department).Id));
        }

        private void ExecuteCreateUserCommand(object obj)
        {
            try
            {
                User.StatusId = 2;
                if (SelectedChat != null)
                    User.Chats.Add(SelectedChat);
                CheckDemands(User);
                _context.Users.Add(User);
                _context.SaveChanges();
                MessageBox.Show("Пользователь добавлен " + User.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckDemands(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
                throw new Exception("Имя не введено");
            if (string.IsNullOrWhiteSpace(user.LastName))
                throw new Exception("Фамилия не введена");
            if (string.IsNullOrWhiteSpace(user.Login))
                throw new Exception("Логин не введен");
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new Exception("Пароль не введен");
            if (!string.IsNullOrWhiteSpace(user.Email))
                if (!Email.IsValidEmail(user.Email))
                    throw new Exception("Не правильная почта");
            if (user.DepartmentId == 0)
                throw new Exception("Не выбран отдел");
            if (user.PostId == 0)
                throw new Exception("Не выбрана должность");
            if (user.Chats.Any() == false)
                throw new Exception("Не выбрана группа");
            if (user.UserTypeId == 0)
                throw new Exception("Не выбран тип пользователя");
        }
    }
}
