using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Classes
{
    internal static class UploadDownloadFile
    {
        //private static YandexDisk yandexClient = new YandexDisk();
        //public static async void UploadFileAsynce(FileInfo fileInfo) => await yandexClient.UploadFileAsync(fileInfo);
        public static async void DownloadFileAsynce(string filename)
        {
            YandexDisk yandexClient = new YandexDisk();
            string file_URL = await yandexClient.DownloadFileAsync(filename);
            Process.Start(new ProcessStartInfo { FileName = file_URL, UseShellExecute = true });
            //using (var client = new System.Net.WebClient())
            //{
            //    string? filename = GetFileName();
            //    if (filename != null)
            //        client.DownloadFileAsync(new Uri(file_URL), filename);
            //}
        }
        public static FileInfo? GetFileInfo()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Все файлы (*.*)|*.*";
                //ofd.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg;*.jpg";
                if (ofd.ShowDialog() == true)
                    return new FileInfo(ofd.FileName); //Path.GetFullPath()
                return null;
            }
            catch
            {
                throw new Exception("Файл не читается");
            }
        }

        //private static string? GetFileName()
        //{
        //    SaveFileDialog sfd = new SaveFileDialog();
        //    sfd.Filter = "Все файлы (*.*)|*.*";
        //    sfd.Title = "Сохранение";
        //    //sfd.FileName = "Sdf.jpg";
        //    //sfd.AddExtension = true;
        //    sfd.ShowDialog();

        //    if (sfd.FileName != "")
        //    {
        //        return sfd.FileName;
        //    }
        //    return null;
        //}
    }

}
