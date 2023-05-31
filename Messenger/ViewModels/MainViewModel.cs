namespace Messenger.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentChildView;
        public ViewModelBase CurrentChildView //Текущий выбранный ViewModel
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
    }
}
