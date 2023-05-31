using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Messenger.Classes.Converters
{
    internal class FullNameToBitmapImage : IValueConverter
    {
        /// <summary>
        /// Конвертирование из пути к файлу в изображение
        /// </summary>
        /// <returns>Возвращает изображение если это возможно</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return new BitmapImage(new Uri((string)value));
            }
            catch
            {
                return null;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
