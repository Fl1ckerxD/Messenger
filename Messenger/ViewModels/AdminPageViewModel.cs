using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Messenger.ViewModels
{
    class AdminPageViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        public ICommand OpenUsersCommand { get; }
        public ICommand OpenChatsCommand { get; }
        public ICommand BackCommand { get; }
        public ViewModelBase CurrentChildView
        {
            get => _currentChildView;
            set
            {
                _currentChildView = value;
                OnPropertyChanged();
            }
        }
        public AdminPageViewModel()
        {
            CurrentChildView = new ListUsersViewModel();
            OpenUsersCommand = new RelayCommand(obj => { CurrentChildView = new ListUsersViewModel(); });
            OpenChatsCommand = new RelayCommand(obj => { CurrentChildView = new ListChatsViewModel(); });
            BackCommand = new RelayCommand(obj => { FrameManager.mainFrame.GoBack(); });//ViewModelManager.mainViewModel.CurrentChildView = new MainPageViewModel();
        }
    }
}
