using System.Data.SqlTypes;
using System.Net;
using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using SQLitePCL;
using PuppeteerSharp;
using System.Diagnostics;

namespace HouseSpotter.Utils
{
    public class ScraperClient
    {
        private readonly IConfiguration _configuration;

        ~ScraperClient()
        {
            EndScrape().Wait();
        }
        public ScraperClient(IConfiguration configuration)
        {
            _configuration = configuration;

            SqlConnection.ConnectionString = _configuration.GetValue<string>("Db:ConnectionString") ?? "";
            UsingSql = String.IsNullOrEmpty(SqlConnection.ConnectionString) ? false : true;

            this.SpeedLimit = _configuration.GetValue<int?>("Scraper:SpeedLimit") ?? 100;
        }

        public async Task EndScrape()
        {
            Debug.WriteLine($"[{DateTimeOffset.Now}] Garbage collecting...");

            if (PuppeeteerBrowser != null)
            {
                await PuppeeteerBrowser.CloseAsync();
            }
            if (SqlConnection != null)
            {
                SqlConnection.Close();
            }
            if (HtmlClient != null)
            {
                HtmlClient.Dispose();
                HtmlClientInitialized = false;
            }
            if (PuppeteerPage != null)
            {
                await PuppeteerPage.CloseAsync();
                PuppeteerInitialized = false;
            }
            
            GC.Collect();
        }
        public async Task InitializeHtmlClient()
        {
            // Set up the browser for HtmlClient
            HtmlClientHandler.UseProxy = false;
            HtmlClientHandler.UseDefaultCredentials = true;
            HtmlClientHandler.UseCookies = true;

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
        public bool HtmlClientInitialized = false;
        public HttpClientHandler HtmlClientHandler = new HttpClientHandler();
        public HttpClient HtmlClient = new HttpClient();
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
