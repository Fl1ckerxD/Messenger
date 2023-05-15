using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Messenger.Styles
{
    public partial class ListViews : ResourceDictionary
    {
        public bool IsCurrentUser { get; set; } = true;
        public void Message_MouseEnter(object sender, EventArgs e)
        {
            ListViewItem listItem = sender as ListViewItem;
            //ViewModelManager.chatViewModel.IsCurrentUser = true;
        }     
    }
}
