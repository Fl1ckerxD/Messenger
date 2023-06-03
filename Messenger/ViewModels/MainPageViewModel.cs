using System.Collections.ObjectModel;
using System.Windows.Input;
using Messenger.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.ViewModels
{
    internal class MainPageViewModel : ViewModelBase
    {
        private MessengerContext _context; //Контекст базы данных
        public User CurrentUser { get; set; } //Текущий пользователь
        public ICommand ShowProfileCommand { get; } //Команда открытия редактирования профиля
        public ICommand OpenGeneralChat { get; } //Команда открытия общего чата
        public ICommand OpenMainChat { get; } //Команда открытия основного чата
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
            Employees = new ObservableCollection<User>(_context.Users.Where(x => x.DepartmentId == CurrentUser.DepartmentId).Include(x => x.Post).OrderBy(x => x.LastName));
            OpenGeneralChat = new RelayCommand(ExecuteOpenGeneralChat);
            OpenMainChat = new RelayCommand(ExecuteOpenMainChat);
            _chachedEmployees = Employees;
        }

        private void ExecuteOpenMainChat(object obj)
        {
            LoggedUser.chatId = _context.Chats.Where(x => x.DepartmentId == LoggedUser.currentUser.DepartmentId).First().Id;
            ViewModelManager.chatViewModel.RefreshDB(LoggedUser.chatId);
        }

        private void ExecuteOpenGeneralChat(object obj)
        {
            LoggedUser.chatId = 32;
            ViewModelManager.chatViewModel.RefreshDB(32);
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
