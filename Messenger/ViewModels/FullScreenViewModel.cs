using Messenger.Views.UserControls;
using System.Windows.Input;

namespace Messenger.ViewModels
{
    class FullScreenViewModel : ViewModelBase
    {
        public FullScreenImage fullScreenImage; //Страница
        public ICommand DownloadFileCommand { get; set; } //Команда скачивания изображения
        private File _image;
        public File Image //Открывшееся изображение
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged(); fullScreenImage.Visibility = System.Windows.Visibility.Visible; }
        }
        public FullScreenViewModel()
        {
            ViewModelManager.FullScreenViewModel = this;
            DownloadFileCommand = new RelayCommand(ExecuteDownloadFileCommand);
        }
        /// <summary>
        /// Скачивание изображения
        /// </summary>
        /// <param name="obj">Объект типа Model.File</param>
        private void ExecuteDownloadFileCommand(object obj)
        {
            File file = (obj as File);
            UploadDownloadFile.DownloadFile(file.FileData, file.FileName, file.FileExtension);
        }
    }
}
