using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace Messenger.ViewModels
{
    class ChatViewModel : ViewModelBase
    {
        private MessengerContext _context;
       // public ObservableCollection<ChatState> States { get; set; }
        public ICommand SendCommand { get; }
        //public ICommand PreviousPageCommand { get; }
        //private bool hasRespondent;
        private Chat _selectedChat;
        public Chat SelectedChat
        {
            get => _selectedChat;
            set
            {
                try
                {
                    _selectedChat = value;
                   // IsOpen = value.StateId == 1 ? true : false;
                  //  hasRespondent = value.RespondentId is not null;
                  //  SelectedId = value.StateId;
                    OnPropertyChanged();
                }
                catch
                {
                    MessageBox.Show("Ошибка подключения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private string _message;
        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        //private bool _isOpen;

        //public bool IsOpen
        //{
        //    get { return _isOpen; }
        //    set { _isOpen = value; OnPropertyChanged(); }
        //}

        private int _selectedId;

        public int SelectedId
        {
            get { return _selectedId; }
            set { _selectedId = value; OnPropertyChanged(); }//ChangeState();
        }

        public ChatViewModel()
        {
            using(var context = new MessengerContext())
            {
                SelectedChat = context.Chats.Include(x => x.Messages).Include(x => x.Users).First();
            }

            ViewModelManager.chatViewModel = this;
            _context = new MessengerContext();
            //try
            //{
            //   // States = new ObservableCollection<ChatState>(_context.ChatStates);
            //}
            //catch
            //{
            //    MessageBox.Show("Ошибка подключения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

            SendCommand = new RelayCommand(ExecuteSendCommand);
            //PreviousPageCommand = new RelayCommand(ExecutePreviousPageCommand);
        }
        //private void ExecutePreviousPageCommand(object obj)
        //{
        //    ViewModelManager.mainViewModel.ShowMainSupportView();
        //}

        private void ExecuteSendCommand(object obj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Message))
                    return;

                _context.Chats.Where(x => x.Id == SelectedChat.Id).First().Messages.Add(new Message
                {
                    Content = Message.Trim(),  
                    User = LoggedUser.currentUser,
                    Time = DateTime.Now
                });

                _context.SaveChanges();
                Message = "";

               // SetRespondent();
            }
            catch
            {
                MessageBox.Show("Не удалось отправить сообщение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //private void SetRespondent()
        //{
        //    try
        //    {
        //        if (!hasRespondent && (LoggedUser.userType == LoggedUserType.Admin || LoggedUser.userType == LoggedUserType.Employee))
        //        {
        //            _context.Chats.Where(x => x.Id == SelectedChat.Id).First().RespondentId = LoggedUser.userId;
        //            _context.SaveChanges();
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Не удалось назначить отвественного", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        //private void ChangeState()
        //{
        //    try
        //    {
        //        _context.Chats.Where(x => x.Id == SelectedChat.Id).First().StateId = SelectedId;
        //        _context.SaveChanges();
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Не удалось изменить статус чата", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
    }
}

