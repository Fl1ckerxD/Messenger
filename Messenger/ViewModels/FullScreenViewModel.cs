using Messenger.Views.UserControls;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Messenger.ViewModels
{
    class FullScreenViewModel : ViewModelBase
    {
        public FullScreenImage fullScreenImage;
        public ICommand DownloadFileCommand { get; set; }

        private Models.File _image;
        public Models.File Image
        {
            get { return _image; }
            set { _image = value; OnPropertyChanged(); fullScreenImage.Visibility = System.Windows.Visibility.Visible; }
        }
        public FullScreenViewModel()
        {
            ViewModelManager.FullScreenViewModel = this;
            DownloadFileCommand = new RelayCommand(ExecuteDownloadFileCommand);
        }
        private void ExecuteDownloadFileCommand(object obj)
        {
            //UploadDownloadFile.DownloadFileAsynce((obj as Models.File).FileName);
            Models.File file = (obj as Models.File);
            UploadDownloadFile.DownloadFile(file.FileData, file.FileName, file.FileExtension);
        }
    }
}
