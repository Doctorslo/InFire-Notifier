using HtmlAgilityPack;
using System.Net;

namespace Notifier.Notifier
{
    internal class InFire
    {
        private readonly HttpClient mHttpClient;
        const string BaseUrl = "https://infire.si";

        public InFire(UserData userData)
        {
            var handler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer()
            };
            handler.CookieContainer.Add(new Uri(BaseUrl), new Cookie("uid", userData.Uid));
            handler.CookieContainer.Add(new Uri(BaseUrl), new Cookie("pass", userData.Pass));
            mHttpClient = new HttpClient(handler);
            mHttpClient.DefaultRequestHeaders.UserAgent.Clear();
            mHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userData.UserAgent);
        }

        public async Task<List<string>> GetTorrents()
        {
            return [.. await GetNormalTorrents(), .. await GetXXXTorrents()];
        }

        private async Task<List<string>> GetNormalTorrents()
        {
            var response = await mHttpClient.GetAsync($"{BaseUrl}/torrents.php");
            return ReadTorrentsFromResponse(response);
        }

        private async Task<List<string>> GetXXXTorrents()
        {
            var response = await mHttpClient.GetAsync($"{BaseUrl}/xxx.php");
            return ReadTorrentsFromResponse(response);
        }

        private List<string> ReadTorrentsFromResponse(HttpResponseMessage response)
        {
            var stringContent = response.Content.ReadAsStringAsync();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(stringContent.Result);
            var aWithClass = htmlDoc.DocumentNode.SelectNodes("//a[contains(concat(' ', normalize-space(@class), ' '), ' tx-inverse tx-medium d-block ')]");
            var torrentNames = new List<string>();
            if (aWithClass == null)
            {
                NotificationHelper.ShowToastNotification("Error", "You are not logged in or website has changed.");
            }
            else
            {
                foreach (var a in aWithClass)
                {
                    torrentNames.Add(a.InnerText);
                }
            }
            return torrentNames;
        }
    }
}
