using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.ViewModels
{
    internal abstract class ListViewModel : ViewModelBase
    {
        private string _textToFilter;
        public string TextToFilter
        {
            get { return _textToFilter; }
            set { _textToFilter = value; OnPropertyChanged(); Search(value); }
        }
        protected abstract void Search(string search);
    }
}
