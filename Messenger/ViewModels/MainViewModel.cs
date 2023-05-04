using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Messenger.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;

        public MainViewModel()
        {
            ViewModelManager.mainViewModel = this;
        }
        public ViewModelBase CurrentChildView
        {
            get => _currentChildView;
            set
            {
                _currentChildView = value;
                OnPropertyChanged();
            }
        }

        internal void OpenMainPage()
        {
            CurrentChildView = new MainPageViewModel();
        }

        internal void OpenUserProfile()
        {
            CurrentChildView = new UserProfileViewModel();
        }
    }
}
