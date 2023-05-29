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
    class ListChatsViewModel : ListViewModel
    {
        private MessengerContext _context;
        private ObservableCollection<Chat> _chachedChats;
        private ObservableCollection<Chat> _chats;
        public ICommand OpenCommand { get; }
        public ObservableCollection<Chat> Chats
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
        protected override void Search(string search)
        {
            Chats = new ObservableCollection<Chat>(_chachedChats.Where(x => x.Theme.Contains(search, StringComparison.OrdinalIgnoreCase)
            || x.Department.Title.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
