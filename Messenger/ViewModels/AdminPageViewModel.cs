using System.Windows.Input;

namespace Messenger.ViewModels
{
    class AdminPageViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        public ICommand OpenUsersCommand { get; } //Команда открытия страницы с пользователями
        public ICommand OpenChatsCommand { get; } //Команда открытия страницы с чатами
        public ICommand BackCommand { get; } //Команда возвращающая на предыдущую страницу
        public ViewModelBase CurrentChildView //Текущий выбранный ViewModel
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
            BackCommand = new RelayCommand(obj => { FrameManager.mainFrame.GoBack(); });
        }
    }
}
