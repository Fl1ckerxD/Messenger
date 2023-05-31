using System.Collections.ObjectModel;
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
        private MessengerContext _context; //Контекст базы данных
        private bool _isEdit = false; //Является ли сообщение редактируемым
        private Message _editMessage; //Редатируемое сообщение
        #endregion
        #region Public
        public ICommand SendCommand { get; } //Команда отправления сообщения
        public ICommand AttachFileCommand { get; } //Команда прикрепления файла к сообщения
        public ICommand DeleteFileCommand { get; } //Команда удаления прикрепленного файла
        public ICommand DownloadFileCommand { get; } //Команда скачевания файла из сообщения
        public ICommand ShowProfileCommand { get; } //Команда открывающая информацию о пользователе
        public ICommand DeleteMessageCommand { get; } //Команда удаления сообщения
        public ICommand OpenImageCommand { get; } //Команда открытия изображения из сообщения
        public ICommand EditMessageCommand { get; } //Команда редактирования сообщения
        #endregion
        #endregion
        #region Public properties
        private Chat _selectedChat;
        public Chat SelectedChat //Открытый чат
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
        public ObservableCollection<FileInfo> AttachedFiles //Список прикрепленных файлов
        {
            get { return _attachedFiles; }
            set { _attachedFiles = value; OnPropertyChanged(); }
        }
        private string _message;
        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        } //Введенное сообщение
        #endregion
        public ChatViewModel()
        {
            _context = new MessengerContext();
            RefreshDB();
            ViewModelManager.chatViewModel = this;

            SendCommand = new RelayCommand(ExecuteSendCommand);
            AttachFileCommand = new RelayCommand(ExecuteAttachFileCommand);
            DeleteFileCommand = new RelayCommand(obj => { AttachedFiles.Remove(obj as FileInfo); });
            DownloadFileCommand = new RelayCommand(ExecuteDownloadFileCommand);
            ShowProfileCommand = new RelayCommand(obj => { ViewModelManager.UserInfoControl.CurrentUser = (obj as User); });
            OpenImageCommand = new RelayCommand(obj => { ViewModelManager.FullScreenViewModel.Image = (obj as Models.File); });
            DeleteMessageCommand = new RelayCommand(ExecuteDeleteMessageCommand);
            EditMessageCommand = new RelayCommand(ExecuteEditMessageCommand);
        }
        /// <summary>
        /// Обновление чата
        /// </summary>
        private void RefreshDB()
        {
            SelectedChat = _context.Chats
                            .Where(x => x.Users.Contains(LoggedUser.currentUser))
                            .Include(x => x.Messages).ThenInclude(x => x.Files)
                            .Include(x => x.Messages).ThenInclude(x => x.User)
                            .First();
        }
        /// <summary>
        /// Редактирование сообщения
        /// </summary>
        /// <param name="obj">Объект типа Model.Message</param>
        private void ExecuteEditMessageCommand(object obj)
        {
            _editMessage = (obj as Message);
            Message = _editMessage.Content;
            _isEdit = true;
        }
        /// <summary>
        /// Удаление сообщения
        /// </summary>
        /// <param name="obj">Объект типа Model.Message</param>
        private void ExecuteDeleteMessageCommand(object obj)
        {
            var Result = MessageBox.Show("Удалить?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.Yes)
            {
                using (var context = new MessengerContext())
                {
                    Message message = obj as Message;
                    context.Entry(message).State = EntityState.Deleted;
                    context.Messages.Remove(message);
                    context.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Скачивание файла из сообщения
        /// </summary>
        /// <param name="obj">Объект типа Model.File</param>
        private void ExecuteDownloadFileCommand(object obj)
        {
            Models.File file = (obj as Models.File);
            UploadDownloadFile.DownloadFile(file.FileData, file.FileName, file.FileExtension);
        }
        /// <summary>
        /// Прикрепление файла к сообщению
        /// </summary>
        private void ExecuteAttachFileCommand(object obj)
        {
            FileInfo? fileInfo = UploadDownloadFile.GetFileInfo();
            if (fileInfo is null)
                return;
            AttachedFiles.Add(fileInfo);
        }
        /// <summary>
        /// Сохранение сообщения
        /// </summary>
        private async void ExecuteSendCommand(object obj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Message) && AttachedFiles.Count == 0)
                {
                    _isEdit = false;
                    return;
                }
                using (var context = new MessengerContext())
                {
                    if (!_isEdit)
                        SendMessage();
                    else
                        SaveEditMessage();
                }
            }
            catch
            {
                MessageBox.Show("Не удалось отправить сообщение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Сохранение нового сообщения в базе данных
        /// </summary>
        private void SendMessage()
        {
            using (var context = new MessengerContext())
            {
                //SelectedChat.Messages.Add(new Message
                _context.Chats.Where(x => x.Id == SelectedChat.Id).First().Messages.Add(new Message
                {
                    Content = Message != null ? Message.Trim() : "",
                    //User = LoggedUser.currentUser,
                    UserId = LoggedUser.currentUser.Id,
                    Files = GetFiles(AttachedFiles),
                    Time = DateTime.Now
                });
                _context.SaveChanges();
                Message = "";
                AttachedFiles.Clear();
            }
        }
        /// <summary>
        /// Сохранение измененного сообщения в базе данных
        /// </summary>
        private void SaveEditMessage()
        {
            using (var context = new MessengerContext())
            {
                _editMessage.Content = Message != null ? Message.Trim() : "";
                _editMessage.Files = GetFiles(AttachedFiles);
                context.Messages.Update(_editMessage);
                context.SaveChanges();
                Message = "";
                AttachedFiles.Clear();
                _isEdit = false;
            }
            SelectedChat = new Chat();
            RefreshDB();
        }
        /// <summary>
        /// Преобразование из IList<FileInfo> в List<Models.File>
        /// </summary>
        private List<Models.File> GetFiles(IList<FileInfo> fileInfos)
        {
            List<Models.File> files = new List<Models.File>();
            if (fileInfos.Count == 0)
                return files;
            foreach (var file in fileInfos)
            {
                files.Add(new Models.File
                {
                    FileName = file.Name,
                    FileExtension = file.Extension,
                    FileData = System.IO.File.ReadAllBytes(file.FullName),
                    FileLength = file.Length
                });
            }
            return files;
        }
    }
}

