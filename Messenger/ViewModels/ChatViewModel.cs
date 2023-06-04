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
        public ICommand CheckUserAccess { get; } //Команда проверки доступа к сообщению
        public Visibility BorderVisibility { get; set; } = Visibility.Hidden;
        #endregion
        #endregion
        #region Public properties
        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages //Список сообщений
        {
            get { return _messages; }
            set { _messages = value; OnPropertyChanged(); }
        }
        private ObservableCollection<FileInfo> _attachedFiles = new ObservableCollection<FileInfo>();
        public ObservableCollection<FileInfo> AttachedFiles //Список прикрепленных файлов
        {
            get { return _attachedFiles; }
            set { _attachedFiles = value; OnPropertyChanged(); }
        }
        private string _message;
        public string Message //Введенное сообщение
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }
        #endregion
        public ChatViewModel()
        {
            ViewModelManager.chatViewModel = this;
            _context = new MessengerContext();
            RefreshDB(LoggedUser.chatId);

            SendCommand = new RelayCommand(ExecuteSendCommand);
            AttachFileCommand = new RelayCommand(ExecuteAttachFileCommand);
            DeleteFileCommand = new RelayCommand(obj => { AttachedFiles.Remove(obj as FileInfo); });
            DownloadFileCommand = new RelayCommand(ExecuteDownloadFileCommand);
            ShowProfileCommand = new RelayCommand(obj => { ViewModelManager.UserInfoControl.CurrentUser = (obj as User); });
            OpenImageCommand = new RelayCommand(obj => { ViewModelManager.FullScreenViewModel.Image = (obj as Models.File); });
            DeleteMessageCommand = new RelayCommand(ExecuteDeleteMessageCommand);
            EditMessageCommand = new RelayCommand(ExecuteEditMessageCommand);
            CheckUserAccess = new RelayCommand(ExecuteCheckUserAccess);
        }
        /// <summary>
        /// Обновление чата
        /// </summary>
        public void RefreshDB(int chatid)
        {
            Messages = new ObservableCollection<Message>(_context.Messages.Where(x => x.ChatId == chatid)
            .Include(x => x.User)
            .Include(x => x.Files));
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
            _context.Messages.Add(new Message
            {
                Content = Message != null ? Message.Trim() : "",
                ChatId = LoggedUser.chatId,
                UserId = LoggedUser.currentUser.Id,
                Files = GetFiles(AttachedFiles),
                Time = DateTime.Now
            });
            _context.SaveChanges();
            Message = "";
            AttachedFiles.Clear();
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
            RefreshDB(LoggedUser.chatId);
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
        /// <summary>
        /// Проверяет тип пользователя выбранного сообщения
        /// </summary>
        /// <param name="obj"></param>
        public void ExecuteCheckUserAccess(object obj)
        {
            var message = obj as Message;
            if (message.User.Id == LoggedUser.currentUser.Id || LoggedUser.userType.GetHashCode() != 0)
                BorderVisibility = Visibility.Visible;
            else BorderVisibility = Visibility.Hidden;
        }
    }
}

