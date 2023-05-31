using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Messenger.ViewModels
{
    internal class CreateUserViewModel : ViewModelBase
    {
        private MessengerContext _context; //Контекст базы данных
        #region Public members
        public User User { get; set; } //Создаваемый пользователь
        public Chat SelectedChat { get; set; } //Выбранный чат
        public ObservableCollection<Department> Departments { get; set; } //Выбранный отдел
        public ObservableCollection<UserType> UserTypes { get; set; } //Выбранный тип пользователя
        public ICommand CreateUserCommand { get; } //Команда создания пользователя
        public ICommand SaveUserCommand { get; } //Команда Сохранения изменений пользователя
        public ICommand SelectedItemChangedCommand { get; } //Команда меняющий items в ComboBoxes
        public ICommand BackCommand { get; } //Команда возвращающая на предыдущую страницу
        public Visibility SignUpVisibility { get; set; } = Visibility.Collapsed; //Видемость кнопки регистрации
        public Visibility EditVisibility { get; set; } = Visibility.Collapsed; //Видемость кнопки сохранения
        #endregion
        #region Properties
        private ObservableCollection<Post> _posts;
        public ObservableCollection<Post> Posts //Список должностей
        {
            get => _posts;
            set { _posts = value; OnPropertyChanged(); }
        } 

        private ObservableCollection<Chat> _chats;
        public ObservableCollection<Chat> Chats //Список чатов
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
            BackCommand = new RelayCommand(obj => { FrameManager.mainFrame.GoBack(); });
            CreateUserCommand = new RelayCommand(ExecuteCreateUserCommand);
            SelectedItemChangedCommand = new RelayCommand(ExecuteSelectedItemChangedCommand);
            SaveUserCommand = new RelayCommand(ExecuteSaveUserCommand);
        }
        /// <summary>
        /// Сохранение изменений пользователя
        /// </summary>
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
        /// <summary>
        /// Изменение items в ComboBoxes
        /// </summary>
        private void ExecuteSelectedItemChangedCommand(object obj)
        {
            Posts = new ObservableCollection<Post>(_context.Posts.Where(x => x.Departments.Contains((obj as Department))));
            Chats = new ObservableCollection<Chat>(_context.Chats.Where(x => x.Department == (obj as Department)));
        }
        /// <summary>
        /// Создание нового пользователя
        /// </summary>
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
        /// <summary>
        /// Проверка требований
        /// </summary>
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
