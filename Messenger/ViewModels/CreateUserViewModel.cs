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
    internal class CreateUserViewModel : ViewModelBase
    {
        private MessengerContext _context;
        #region Public members
        public User User { get; set; }
        public Chat SelectedChat { get; set; }
        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<UserType> UserTypes { get; set; }
        public ICommand CreateUserCommand { get; }
        public ICommand SaveUserCommand { get; }
        public ICommand SelectedItemChangedCommand { get; }
        public ICommand BackCommand { get; }
        public Visibility SignUpVisibility { get; set; } = Visibility.Collapsed;
        public Visibility EditVisibility { get; set; } = Visibility.Collapsed;
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
        public CreateUserViewModel(User user, bool isSave = false)
        {
            _context = new MessengerContext();
            if (user != null)
            {
                User = _context.Users.Where(x => x.Id == user.Id).Include(x => x.Department)
                    .Include(x => x.Post)
                    .Include(x => x.Status)
                    .Include(x => x.Chats)
                    .First();
                SelectedChat = User.Chats.First();
                ExecuteSelectedItemChangedCommand(User.Department);
            }
            else
                User = new User();
            if (isSave)
                EditVisibility = Visibility.Visible;
            else
                SignUpVisibility = Visibility.Visible;
            Departments = new ObservableCollection<Department>(_context.Departments);
            UserTypes = new ObservableCollection<UserType>(_context.UserTypes);
            BackCommand = new RelayCommand(obj => { FrameManager.mainFrame.GoBack(); });//ViewModelManager.mainViewModel.CurrentChildView = new MainPageViewModel();
            CreateUserCommand = new RelayCommand(ExecuteCreateUserCommand);
            SelectedItemChangedCommand = new RelayCommand(ExecuteSelectedItemChangedCommand);
            SaveUserCommand = new RelayCommand(ExecuteSaveUserCommand);
        }

        private void ExecuteSaveUserCommand(object obj)
        {
            try
            {
                if (SelectedChat != null)
                {
                    var list = User.Chats.ToList();
                    list.Remove(User.Chats.First());
                    list.Add(SelectedChat);
                    User.Chats = list;
                }
                    
                CheckDemands(User);
                _context.SaveChanges();
                MessageBox.Show("Изменения сохранены");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteSelectedItemChangedCommand(object obj)
        {
            Posts = new ObservableCollection<Post>(_context.Posts.Where(x => x.Departments.Contains((obj as Department))));
            Chats = new ObservableCollection<Chat>(_context.Chats.Where(x => x.Department == (obj as Department)));
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
                MessageBox.Show($"Пользователь {User.Login} добавлен");
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
                if (!ValidProperty.IsValidEmail(user.Email))
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
