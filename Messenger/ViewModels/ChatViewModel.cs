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
        public ICommand DownloadFileCommand { get; }

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
                SelectedChat = context.Chats
                    .Include(x => x.Messages).ThenInclude(x => x.Files)
                    .Include(x => x.Users)
                    .First();
            }

            ViewModelManager.chatViewModel = this;
            _context = new MessengerContext();

            SendCommand = new RelayCommand(ExecuteSendCommand);
            AttachFileCommand = new RelayCommand(ExecuteAttachFileCommand);
            DeleteFileCommand = new RelayCommand(obj => { AttachedFiles.Remove(obj as FileInfo); });
            DownloadFileCommand = new RelayCommand(ExecuteDownloadFileCommand);
        }

        private void ExecuteDownloadFileCommand(object obj)
        {
            UploadDownloadFile.DownloadFileAsynce((obj as Models.File).FileName);
        }

        private void ExecuteAttachFileCommand(object obj)
        {
            FileInfo? fileInfo = UploadDownloadFile.GetFileInfo();
            if (fileInfo is null)
                return;
            AttachedFiles.Add(fileInfo);
        }

        private async void ExecuteSendCommand(object obj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Message) && AttachedFiles.Count == 0)
                    return;

                _context.Chats.Where(x => x.Id == SelectedChat.Id).First().Messages.Add(new Message
                {
                    Content = Message.Trim(),
                    User = _context.Users.First(),//LoggedUser.currentUser
                    Files = await GetFilesAsync(AttachedFiles),
                    Time = DateTime.Now
                });

                _context.SaveChanges();
                Message = "";
                AttachedFiles.Clear();
            }
            catch
            {
                MessageBox.Show("Не удалось отправить сообщение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<List<Models.File>> GetFilesAsync(IList<FileInfo> fileInfos)
        {
            List<Models.File> files = new List<Models.File>();
            if (fileInfos.Count == 0)
                return files;

            YandexDisk yandexClient = new YandexDisk();

            foreach (var file in fileInfos)
            {
                yandexClient.UploadFileAsync(file);
                files.Add(new Models.File
                {
                    FileName = file.Name,
                    FileUrl = await yandexClient.DownloadFileAsync(file.Name)
                });
            }
            return files;
        }
    }
}

