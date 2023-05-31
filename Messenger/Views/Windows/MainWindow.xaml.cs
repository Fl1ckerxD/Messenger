using System.Windows;

namespace Messenger.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
            InitializeComponent();
            FrameManager.mainFrame = mainFrame;
        }
        /// <summary>
        /// Свернуть окно
        /// </summary>
        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// Открывает окно во весь экран монитора
        /// </summary>
        private void MaximizeRestoreClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }
        /// <summary>
        /// Закрытие окна
        /// </summary>
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
