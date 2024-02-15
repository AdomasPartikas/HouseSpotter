using System;
using System.Diagnostics;
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
        private readonly InMemoryLoggerProvider _loggerProvider;
        private readonly ScraperClient _scraperClient;
        private readonly ScraperForAruodas _scraperForAruodas;
        public List<string>? LogMessages { get; set; }
        public ScrapeModel(ILogger<ScrapeModel> logger, ScraperClient scraperClient, ScraperForAruodas scraperForAruodas, InMemoryLoggerProvider loggerProvider)
        {
            _logger = logger;
            _scraperClient = scraperClient;
            _scraperForAruodas = scraperForAruodas;
            _loggerProvider = loggerProvider;
        }
        public async Task OnGetAsync()
        {
            Debug.WriteLine($"[{DateTimeOffset.Now}] Scrape page loaded");
        }
        public async Task<IActionResult> OnPostFindHousingAsync()
        {
            var logMessages = _loggerProvider.LogMessages;
            LogMessages = logMessages.ToList();
            _logger.LogInformation($"[{DateTimeOffset.Now}] Scrape started");
            try
            {
                await _scraperForAruodas.FindHousing("namai");
                //await _scraperForAruodas.FindHousing("namu-nuoma");
                //await _scraperForAruodas.FindHousing("butai");
                //await _scraperForAruodas.FindHousing("butu-nuoma");

                await _scraperClient.EndScrape();
                GC.Collect();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{DateTimeOffset.Now}] Unknown error occurred during web scraping");
            }
            _logger.LogInformation($"[{DateTimeOffset.Now}] Scrape finished");

            // Redirect to the same page or to a confirmation page after the action completes
            return RedirectToPage();
        }
    }
}
