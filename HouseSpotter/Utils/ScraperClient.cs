using System.Data.SqlTypes;
using System.Net;
using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using SQLitePCL;
using PuppeteerSharp;

namespace HouseSpotter.Utils
{
    public class ScraperClient
    {
        private readonly IConfiguration _configuration;

        ~ScraperClient()
        {
            if (PuppeeteerBrowser != null)
            {
                PuppeeteerBrowser.CloseAsync().Wait();
            }
            if(SqlConnection != null)
            {
                SqlConnection.Close();
            }
            if(Client != null)
            {
                Client.Dispose();
            }
            if(PuppeteerPage != null)
            {
                PuppeteerPage.CloseAsync().Wait();
            }
            GC.Collect();
        }
        public ScraperClient(IConfiguration configuration)
        {
            _configuration = configuration;

            SqlConnection.ConnectionString = _configuration.GetValue<string>("Db:ConnectionString") ?? "";
            UsingSql = String.IsNullOrEmpty(SqlConnection.ConnectionString) ? false : true;

            this.SpeedLimit = _configuration.GetValue<int?>("Scraper:SpeedLimit") ?? 100;

            // Set up the browser for HtmlClient
            ClientHandler.UseProxy = false;
            ClientHandler.UseDefaultCredentials = true;
            ClientHandler.UseCookies = true;

            Client = new HttpClient(ClientHandler);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            Client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgents.GetRandomUserAgent());
            Client.DefaultRequestHeaders.Accept.Clear();
            //Client.DefaultRequestHeaders.Add("accept", @"text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            Client.DefaultRequestHeaders.Add("upgrade-insecure-requests", "1");
            Client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            Client.DefaultRequestHeaders.Add("sec-ch-ua-arch", "x86");
            Client.DefaultRequestHeaders.Add("sec-ch-ua-wow64", "?0");
            Client.DefaultRequestHeaders.Add("sec-ch-ua-bitness", "112.0.5615.165");
            Client.DefaultRequestHeaders.Add("sec-ch-ua-full-version", "64");
            Client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Linux");
            Client.DefaultRequestHeaders.Add("sec-ch-ua-platform-version", "5.14.0");
            Client.DefaultRequestHeaders.Add("sec-fetch-site", "none");
            Client.DefaultRequestHeaders.Add("sec-fetch-mode", "navigate");
            Client.DefaultRequestHeaders.Add("sec-fetch-user", "?1");
            Client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"112\", \"Google Chrome\";v=\"112\", \"Not:A-Brand\";v=\"99\"");
            //Set up the browser for HtmlClient
        }

        public async Task InitializePupeeter()
        {
            await new BrowserFetcher().DownloadAsync();

            var launchOptions = new LaunchOptions
            {
                Headless = true
            };

            PuppeeteerBrowser = await Puppeteer.LaunchAsync(launchOptions);
            PuppeteerPage = await PuppeeteerBrowser.NewPageAsync();

            await PuppeteerPage.SetJavaScriptEnabledAsync(true);
            await PuppeteerPage.SetUserAgentAsync(UserAgents.GetRandomUserAgent());

            PuppeteerInitialized = true;
        }

        public bool UsingSql = false;
        public MySqlConnection? SqlConnection = new MySqlConnection();
        public MySqlCommand? SqlCommand;
        public HttpClientHandler ClientHandler = new HttpClientHandler();
        public HttpClient Client = new HttpClient();
        public IBrowser? PuppeeteerBrowser;
        public IPage? PuppeteerPage;
        public bool PuppeteerInitialized = false;
        public int Queries;
        public int? SpeedLimit;
        public DateTime PageScanStartDate;
        public DateTime PageScanEndDate;
        public bool SkipPage = false;
        public TimeSpan TotalScrapeTimer;
    }
}
