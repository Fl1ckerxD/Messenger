using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        //private void showPassword_Checked(object sender, RoutedEventArgs e)
        //{
        //    tb_Password.Text = pb_Password.Password;
        //    tb_Password.Visibility = Visibility.Visible;
        //    pb_Password.Visibility = Visibility.Collapsed;
        //}

        //private void showPassword_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    pb_Password.Password = tb_Password.Text;
        //    tb_Password.Visibility = Visibility.Collapsed;
        //    pb_Password.Visibility = Visibility.Visible;
        //}

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            ViewModelManager.mainViewModel.OpenMainPage();
        }

        //private void Page_Loaded(object sender, RoutedEventArgs e)
        //{
        //    pb_Password.Password = tb_Password.Text;
        //}

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string imagePath = GetImagePath();
                imageUser.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(imagePath)), Stretch = Stretch.UniformToFill };
                (this.DataContext as UserProfileViewModel).CurrentUser.Image = System.IO.File.ReadAllBytes(imagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string GetImagePath()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg;*.jpg";
                if (ofd.ShowDialog() == true)
                    return System.IO.Path.GetFullPath(ofd.FileName);
                return imageUser.Fill.ToString();
            }
            catch
            {
                throw new Exception("Файл не читается");
            }
        }
        private readonly Regex regex = new Regex("^[0-9+]+$");
        private void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            //e.Handled = !IsTextAllowed(e.Text, @"[1]");
        }
        //private static bool IsTextAllowed(string Text, string AllowedRegex)
        //{
        //    try
        //    {
        //        var regex = new Regex(AllowedRegex);
        //        return !regex.IsMatch(Text);
        //    }
        //    catch
        //    {
        //        return true;
        //    }
        //}
    }
}
