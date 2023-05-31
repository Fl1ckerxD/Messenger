namespace Messenger.ViewModels
{
    internal abstract class ListViewModel : ViewModelBase
    {
        private string _textToFilter;
        public string TextToFilter //Текст введенный пользователем
        {
            get { return _textToFilter; }
            set { _textToFilter = value; OnPropertyChanged(); Search(value); }
        }
        protected abstract void Search(string search); //Метов для поиска
    }
}
