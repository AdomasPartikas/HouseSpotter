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

            NetworkHttpClient = new NetworkHttpClient();
            NetworkPuppeteerClient = new NetworkPuppeteerClient();
        }

        public async Task EndScrape()
        {
            Debug.WriteLine($"[{DateTimeOffset.Now}] Garbage collecting...");

            if (SqlConnection != null)
            {
                SqlConnection.Close();
            }
            if (NetworkHttpClient != null)
            {
                await NetworkHttpClient.Destroy();
            }
            if(NetworkPuppeteerClient != null)
            {
                await NetworkPuppeteerClient.Destroy();
            }
            GC.Collect();
            
            Debug.WriteLine($"[{DateTimeOffset.Now}] Garbage collected");
        }

        public bool UsingSql = false;
        public MySqlConnection? SqlConnection = new MySqlConnection();
        public MySqlCommand? SqlCommand;
        public NetworkHttpClient NetworkHttpClient;
        public NetworkPuppeteerClient NetworkPuppeteerClient;
        public int Queries;
        public int? SpeedLimit;
        public DateTime PageScanStartDate;
        public DateTime PageScanEndDate;
        public bool SkipPage = false;
        public TimeSpan TotalScrapeTimer;
    }
}
