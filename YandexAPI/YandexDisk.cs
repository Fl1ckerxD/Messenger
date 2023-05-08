using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace YandexAPI
{
    public class YandexDisk
    {
        private const string API_URL = "https://cloud-api.yandex.net/v1/";
        private const string TOKEN = "y0_AgAAAABof7dyAAngKwAAAADivpc1b9DO93WAQzWsqwpw5mJy66p-5kQ";

        private HttpClient _client;

        public YandexDisk()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", TOKEN);
        }

        private async Task<string> GetResponseAsync(string url)
        {
            var res = await _client.GetAsync(url);
            return await res.Content.ReadAsStringAsync();
        }

        public async Task UploadFileAsync(FileInfo fileInfo)
        {
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                //Load the file and set the file's Content-Type header
                var fileStreamContent = new StreamContent(fileInfo.OpenRead());
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue($"file/{fileInfo.Extension}");

                //Add the file
                multipartFormContent.Add(fileStreamContent, name: "file", fileName: $"{fileInfo.Name}");

                //Send it
                var res = await _client.PutAsync(await GetUploadLinkAsync(fileInfo.Name), multipartFormContent);
            }
        }

        private async Task<string> GetUploadLinkAsync(string fileName)
        {
            string jsonResult = await GetResponseAsync($"{API_URL}disk/resources/upload?path=%2FMessengerFiles%2F{fileName}&overwrite=true");
            var jsonObject = JObject.Parse(jsonResult);
            return (string)jsonObject["href"];
        }

        public async Task<string> DownloadFileAsync(string fileName)
        {
            string jsonResult = await GetResponseAsync($"{API_URL}disk/resources/download?path=%2FMessengerFiles%2F{fileName}");
            var jsonObject = JObject.Parse(jsonResult);
            return (string)jsonObject["href"];
        }
    }

}