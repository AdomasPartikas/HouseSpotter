using System.Data.SqlTypes;
using MySql.Data.MySqlClient;
using SQLitePCL;

namespace HouseSpotter.Utils
{
    public class ScraperClient
    {
        private readonly IConfiguration _configuration;
        public ScraperClient(IConfiguration configuration)
        {
            _configuration = configuration;

            SqlConnection.ConnectionString = _configuration.GetValue<string>("Db:ConnectionString") ?? null;
            UsingSql = String.IsNullOrEmpty(SqlConnection.ConnectionString) ? false : true;

            this.SpeedLimit = _configuration.GetValue<int?>("Scraper:SpeedLimit") ?? 10;

            ClientHandler.UseProxy = false;
            ClientHandler.UseDefaultCredentials = true;
            ClientHandler.UseCookies = true;

            Client = new HttpClient(ClientHandler);
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36");
            Client.DefaultRequestHeaders.Add("Accept", @"text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            Client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            Client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            Client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            Client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            Client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Windows");
            Client.DefaultRequestHeaders.Add("sec-ch-ua-site", "none");
            Client.DefaultRequestHeaders.Add("sec-ch-ua-mode", "navigate");
            Client.DefaultRequestHeaders.Add("sec-ch-ua-user", "?1");
            Client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Not A(Brand\";v=\"99\", \"Google Chrome\";v=\"121\", \"Chromium\";v=\"121\"");
        }

        public bool UsingSql = false;
        public MySqlConnection? SqlConnection;
        public MySqlCommand? SqlCommand;
        public HttpClientHandler ClientHandler = new HttpClientHandler();
        public HttpClient Client = new HttpClient();
        public int Queries;
        public int? SpeedLimit;
        public DateTime PageScanStartDate;
        public DateTime PageScanEndDate;
        public bool SkipPage = false;
        public TimeSpan TotalScrapeTimer;
    }
}