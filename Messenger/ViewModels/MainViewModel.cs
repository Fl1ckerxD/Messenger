using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Messenger.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        public ViewModelBase CurrentChildView
        {
            get => _currentChildView;
            set
            {
                _currentChildView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ViewModelManager.mainViewModel = this;
        }

        internal void OpenMainPage()
        {
            //CurrentChildView = new MainPageViewModel();
        }
    }
}
