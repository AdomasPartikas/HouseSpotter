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
        private readonly ScraperForAruodas _scraperForAruodas;
        public ScrapeModel(ILogger<ScrapeModel> logger, ScraperClient scraperClient, ScraperForAruodas scraperForAruodas)
        {
            _logger = logger;
            _scraperClient = scraperClient;
            _scraperForAruodas = scraperForAruodas;
        }
        public async Task OnGetAsync()
        {
            _logger.LogInformation($"[{DateTimeOffset.Now}] Scrape started");
            try
            {
                await _scraperForAruodas.FindHousing("namai");
                await _scraperForAruodas.FindHousing("namu-nuoma");
                await _scraperForAruodas.FindHousing("butai");
                await _scraperForAruodas.FindHousing("butu-nuoma");

                await _scraperClient.EndScrape();
                GC.Collect();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{DateTimeOffset.Now}] Unknown error occurred during web scraping");
            }
            _logger.LogInformation($"[{DateTimeOffset.Now}] Scrape finished");
        }
    }
}
