using HtmlAgilityPack;
using System.Net;

namespace Notifier
{
    internal class InFire
    {
        private readonly HttpClient mHttpClient;

        public InFire(UserData userData)
        {
            var handler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer()
            };
            handler.CookieContainer.Add(new Uri("https://infire.si"), new Cookie("uid", userData.Uid));
            handler.CookieContainer.Add(new Uri("https://infire.si"), new Cookie("pass", userData.Pass));
            mHttpClient = new HttpClient(handler);
            mHttpClient.DefaultRequestHeaders.UserAgent.Clear();
            mHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userData.UserAgent);
        }

        public async Task<List<string>> GetTorrents()
        {
            var response = await mHttpClient.GetAsync("https://infire.si/torrents.php");
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
