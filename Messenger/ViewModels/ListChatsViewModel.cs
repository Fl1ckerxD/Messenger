using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace Messenger.ViewModels
{
    class ListChatsViewModel : ListViewModel
    {
        private MessengerContext _context; //Контекст базы данных
        private ObservableCollection<Chat> _chachedChats; //Список кэшированого чата
        private ObservableCollection<Chat> _chats; 
        public ICommand OpenCommand { get; } //Команда открытия чата
        public ObservableCollection<Chat> Chats //Список чатов
        {
            get => _chats;
            set { _chats = value; OnPropertyChanged(); }
        }
        public ListChatsViewModel()
        {
            _context = new MessengerContext();
            Chats = new ObservableCollection<Chat>(_context.Chats.Where(x => x.DepartmentId != null)
                .Include(x => x.Department)
                .Include(x => x.Users));
            _chachedChats = Chats;

            OpenCommand = new RelayCommand(obj => { MessageBox.Show((obj as Chat).Theme); });
        }
        /// <summary>
        /// Поиск чатов по названию
        /// </summary>
        /// <param name="search">Строка с текстов для поиска</param>
        protected override void Search(string search)
        {
            Chats = new ObservableCollection<Chat>(_chachedChats.Where(x => x.Theme.Contains(search, StringComparison.OrdinalIgnoreCase)
            || x.Department.Title.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
