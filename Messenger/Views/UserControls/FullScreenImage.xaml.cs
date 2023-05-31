using System.Windows;
using System.Windows.Controls;

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
            Visibility = Visibility.Collapsed;
        }
    }
}
