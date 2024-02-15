using System.Net;

namespace HouseSpotter.Utils
{
    public class NetworkHttpClient
    {
        public async Task Destroy()
        {
            if (HtmlClient != null)
            {
                HtmlClient.Dispose();
            }
            HtmlClientInitialized = false;
        }
        public async Task Initialize()
        {
            // Set up the browser for HtmlClient
            HtmlClientHandler = new HttpClientHandler
            {
                UseProxy = false,
                UseDefaultCredentials = true,
                UseCookies = true
            };

            HtmlClient = new HttpClient(HtmlClientHandler);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            HtmlClient.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgents.GetRandomUserAgent());
            HtmlClient.DefaultRequestHeaders.Accept.Clear();
            //Client.DefaultRequestHeaders.Add("accept", @"text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            HtmlClient.DefaultRequestHeaders.Add("upgrade-insecure-requests", "1");
            HtmlClient.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            HtmlClient.DefaultRequestHeaders.Add("sec-ch-ua-arch", "x86");
            HtmlClient.DefaultRequestHeaders.Add("sec-ch-ua-wow64", "?0");
            HtmlClient.DefaultRequestHeaders.Add("sec-ch-ua-bitness", "112.0.5615.165");
            HtmlClient.DefaultRequestHeaders.Add("sec-ch-ua-full-version", "64");
            HtmlClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Linux");
            HtmlClient.DefaultRequestHeaders.Add("sec-ch-ua-platform-version", "5.14.0");
            HtmlClient.DefaultRequestHeaders.Add("sec-fetch-site", "none");
            HtmlClient.DefaultRequestHeaders.Add("sec-fetch-mode", "navigate");
            HtmlClient.DefaultRequestHeaders.Add("sec-fetch-user", "?1");
            HtmlClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"112\", \"Google Chrome\";v=\"112\", \"Not:A-Brand\";v=\"99\"");
            //Set up the browser for HtmlClient
            HtmlClientInitialized = true;
        }
        public bool HtmlClientInitialized = false;
        public HttpClientHandler? HtmlClientHandler;
        public HttpClient? HtmlClient;
    }
}