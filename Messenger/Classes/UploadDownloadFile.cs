using Microsoft.Win32;
using System.IO;

namespace Messenger.Classes
{
    internal static class UploadDownloadFile
    {
        /// <summary>
        /// Возвращает объект FileInfo, представляющий информацию о файле, выбранный пользователем через диалоговое окно OpenFileDialog
        /// </summary>
        /// <returns>Возвращает объект FileInfo если файл был выбран, иначе null</returns>
        public static FileInfo? GetFileInfo()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Все файлы (*.*)|*.*";
                if (ofd.ShowDialog() == true)
                    return new FileInfo(ofd.FileName);
                return null;
            }
            catch
            {
                throw new Exception("Файл не читается");
            }
        }
        /// <summary>
        /// Скачивает файл на компьютер
        /// </summary>
        /// <param name="buffer">Массив байтов файла</param>
        /// <param name="fileName">Имя файла</param>
        /// <param name="extension">Формат файла</param>
        public static void DownloadFile(byte[] buffer, string fileName, string extension)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = string.Format("{0} файл|*{1}|Все файлы (*.*)|*.*", extension.Substring(1).ToUpper(), extension);
            sfd.Title = "Сохранение";
            sfd.FileName = fileName;
            if (sfd.ShowDialog() == true)
            {
                using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
                    fs.Write(buffer);
            }
        }
    }
}
