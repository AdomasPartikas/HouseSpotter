using System;
using System.Net.Http;
using System.Threading.Tasks;
using HouseSpotter.Scrapers;
using HouseSpotter.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HouseSpotter.Pages
{
    public class ScrapeModel : PageModel
    {
        private readonly ILogger<ScrapeModel> _logger;
        private readonly ScraperClient _scraperClient;
        public ScrapeModel(ILogger<ScrapeModel> logger, ScraperClient scraperClient)
        {
            _logger = logger;
            _scraperClient = scraperClient;
        }
        public async Task OnGetAsync()
        {
            _logger.LogInformation($"[{DateTimeOffset.Now}] Scrape started");
            try
            {
                var aruodasScraper = new ScraperForAruodas(_scraperClient);
                await aruodasScraper.FindApartaments("butai");
                await aruodasScraper.FindApartaments("butu-nuoma");
                _logger.LogInformation($"[{DateTimeOffset.Now}] Scrape completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{DateTimeOffset.Now}] Error occurred during web scraping");
            }
        }
    }
}
