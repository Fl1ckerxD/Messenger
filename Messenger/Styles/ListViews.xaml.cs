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
        }     
    }
}
