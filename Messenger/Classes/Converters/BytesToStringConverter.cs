using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Messenger.Classes.Converters
{
    internal class BytesToStringConverter : IValueConverter
    {
        /// <summary>
        /// Конвертирование размера файла из байтов в килобайты, мегабайты, гигабайты и т.д.
        /// </summary>
        /// <param name="byteCount">Количество байтов</param>
        /// <returns>Возвращает размер файла в килобайты, мегабайты, гигабайты и т.д.</returns>
        public object Convert(object byteCount, Type targetType, object parameter, CultureInfo culture)
        {
            string[] suf = { "Byt", "KB", "MB", "GB", "TB", "PB", "EB" };
            if ((long)byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs((long)byteCount);
            int place = System.Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign((long)byteCount) * num).ToString() + suf[place];
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}