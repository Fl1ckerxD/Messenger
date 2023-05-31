using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Messenger.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Page
    {
        public UserProfile()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обработчик событий запускаемый после нажатия на изображение
        /// </summary>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string imagePath = GetImagePath();
                if (imagePath == string.Empty)
                    return;
                imageUser.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(imagePath)), Stretch = Stretch.UniformToFill };
                (this.DataContext as UserProfileViewModel).CurrentUser.Image = System.IO.File.ReadAllBytes(imagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Получение пути к изображению
        /// </summary>
        /// <returns>Возвращает путь к изображению</returns>
        private string GetImagePath()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg;*.jpg";
                if (ofd.ShowDialog() == true)
                    return System.IO.Path.GetFullPath(ofd.FileName);
                return string.Empty;
            }
            catch
            {
                throw new Exception("Файл не читается");
            }
        }
        private readonly Regex regex = new Regex("^[0-9+]+$"); //Паттерн для ввода проделенных символов
        private void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
        }
    }
}
