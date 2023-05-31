using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Messenger.Classes.Converters
{
    class VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Получение значения Visibility опираясь на типе данных конвертируемого объекта 
        /// </summary>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return Visibility.Hidden;

                }
                else if (value.GetType().Name == "Byte[]")
                {
                    BitmapImage bitmap = ToImage((byte[])value);
                    return Visibility.Visible;
                }
                else
                    return Visibility.Visible;
            }
            catch
            {
                return Visibility.Hidden;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Преобразование массива байтов, представляющих изображение, в объект типа BitmapImage
        /// </summary>
        /// <param name="array">Вес файла</param>
        /// <returns>Возвращает объект типа BitmapImage с преобразованным изображением</returns>
        private BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
    }
}
