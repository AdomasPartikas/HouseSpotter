using System.Data.SqlTypes;
using System.Net;
using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using PuppeteerSharp;
using System.Diagnostics;

namespace HouseSpotter.Server.Utils
{
    /// <summary>
    /// Represents a client for scraping data.
    /// </summary>
    public class ScraperClient
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Finalizer that ensures the scrape is ended before the object is garbage collected.
        /// </summary>
        ~ScraperClient()
        {
            EndScrape().Wait();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScraperClient"/> class.
        /// </summary>
        /// <param name="configuration">The configuration object.</param>
        public ScraperClient(IConfiguration configuration)
        {
            _configuration = configuration;

            this.SpeedLimit = _configuration.GetValue<int?>("Scraper:SpeedLimit") ?? 100;

            NetworkPuppeteerClient = new NetworkPuppeteerClient();
        }

        /// <summary>
        /// Ends the scrape and performs necessary cleanup.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task EndScrape()
        {
            Debug.WriteLine($"[{DateTimeOffset.Now}] Garbage collecting...");

            TotalQueries = 0;
            NewQueries = 0;

            if(NetworkPuppeteerClient != null)
            {
                await NetworkPuppeteerClient.Destroy();
            }
            GC.Collect();
            
            Debug.WriteLine($"[{DateTimeOffset.Now}] Garbage collected");
        }
        /// <summary>
        /// Gets or sets the network Puppeteer client.
        /// </summary>
        public NetworkPuppeteerClient NetworkPuppeteerClient;

        /// <summary>
        /// Gets or sets the number of new queries.
        /// </summary>
        public int NewQueries;

        /// <summary>
        /// Gets or sets the total number of queries.
        /// </summary>
        public int TotalQueries;

        /// <summary>
        /// Gets or sets the speed limit for scraping.
        /// </summary>
        public int? SpeedLimit;

        /// <summary>
        /// Gets or sets the start date of the scrape.
        /// </summary>
        public DateTime ScrapeStartDate;

        /// <summary>
        /// Gets or sets the end date of the scrape.
        /// </summary>
        public DateTime ScrapeEndDate;
    }
}
