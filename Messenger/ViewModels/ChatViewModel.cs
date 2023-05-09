using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Messenger.ViewModels
{
    class ChatViewModel : ViewModelBase
    {
        #region Members

        #region Private
        private MessengerContext _context;
        #endregion

        #region Public
        public ICommand SendCommand { get; }
        public ICommand AttachFileCommand { get; }
        public ICommand DeleteFileCommand { get; }
        #endregion

        #endregion

        #region Public properties

        private Chat _selectedChat;
        public Chat SelectedChat
        {
            get => _selectedChat;
            set
            {
                try
                {
                    _selectedChat = value;
                    OnPropertyChanged();
                }
                catch
                {
                    MessageBox.Show("Ошибка подключения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private ObservableCollection<FileInfo> _attachedFiles = new ObservableCollection<FileInfo>();
        public ObservableCollection<FileInfo> AttachedFiles
        {
            get { return _attachedFiles; }
            set { _attachedFiles = value; OnPropertyChanged(); }
        }

        private string _message;
        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private int _selectedId;
        public int SelectedId
        {
            get { return _selectedId; }
            set { _selectedId = value; OnPropertyChanged(); }
        }

        #endregion

        public ChatViewModel()
        {
            using (var context = new MessengerContext())
            {
                SelectedChat = context.Chats.Include(x => x.Messages).Include(x => x.Users).First();
            }

            ViewModelManager.chatViewModel = this;
            _context = new MessengerContext();

            SendCommand = new RelayCommand(ExecuteSendCommand);
            AttachFileCommand = new RelayCommand(ExecuteAttachFileCommand);
            DeleteFileCommand = new RelayCommand(obj => { AttachedFiles.Remove(obj as FileInfo); });
        }

        private void ExecuteAttachFileCommand(object obj)
        {
            FileInfo? fileInfo = UploadDownloadFile.GetFileInfo();
            if (fileInfo is null)
                return;
            AttachedFiles.Add(fileInfo);
        }

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
            }
            catch
            {
                MessageBox.Show("Не удалось отправить сообщение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

