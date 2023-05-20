using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Messenger.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для FullSrceenImage.xaml
    /// </summary>
    public partial class FullScreenImage : UserControl
    {
        public FullScreenImage()
        {
            InitializeComponent();
            DataContext = new FullScreenViewModel { fullScreenImage = this };
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            //(DataContext as UserInfoViewModel).SelectedUser = null;
            Visibility = Visibility.Collapsed;
        }
    }
}
