using HouseSpotter.Server.Context;
using HouseSpotter.Server.Models;
using HouseSpotter.Server.Utils;
using Microsoft.EntityFrameworkCore;

namespace HouseSpotter.Server.Interfaces
{
    public abstract class ScraperBase : IScraper
    {
        protected ScraperClient _scraperClient;
        protected ILogger _logger;
        protected HousingContext _housingContext;

        protected ScraperBase(ScraperClient scraperClient, ILogger logger, HousingContext housingContext)
        {
            _scraperClient = scraperClient;
            _logger = logger;
            _housingContext = housingContext;
        }

        public abstract Task<Scrape> ScrapeHousingPosts(string pageUrl, string siteEndpoint, Scrape scrape);
        public abstract Task<Scrape> FindHousingPosts(int scrapeDepth);
        public abstract Task GetHousingDetails(Housing house);

        protected async Task EnsurePuppeteerInitialized(string initialUrl)
        {
            if (!_scraperClient.NetworkPuppeteerClient.PuppeteerInitialized)
            {
                await _scraperClient.NetworkPuppeteerClient.Initialize();
                await _scraperClient.NetworkPuppeteerClient.PuppeteerPage!.GetCookiesAsync(initialUrl);
                await Task.Delay(125);
            }
        }

        protected async Task<Scrape> InitializeScrape(ScrapeType scrapeType, ScrapedSite scrapedSite)
        {
            _scraperClient.ScrapeStartDate = DateTime.Now;

            var scrape = new Scrape
            {
                ScrapeType = scrapeType,
                ScrapedSite = scrapedSite,
                ScrapeStatus = ScrapeStatus.Ongoing,
                DateScraped = DateTime.Now
            };

            try
            {
                _housingContext.Scrapes.Add(scrape);
                await _housingContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save scrape progress to database.");
            }

            return scrape;
        }

        protected async Task<Scrape> ScrapeUpdate(Scrape scrape)
        {
            scrape.ScrapeTime = DateTime.Now - _scraperClient.ScrapeStartDate;
            scrape.TotalQueries = _scraperClient.TotalQueries;
            scrape.NewQueries = _scraperClient.NewQueries;
            
            try
            {
                await _housingContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update scrape progress in database.");
            }

            return scrape;
        }

        protected async Task<Scrape> CompleteScrape(Scrape scrape, ScrapeResult scrapeResult, string message)
        {
            _scraperClient.ScrapeEndDate = DateTime.Now;

            scrape.ScrapeTime = _scraperClient.ScrapeEndDate - _scraperClient.ScrapeStartDate;
            scrape.ScrapeResult = scrapeResult;
            scrape.ScrapeStatus = ScrapeStatus.Completed;
            scrape.Message = message;

            try
            {
                await _housingContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update scrape completion status in database.");
            }

            return scrape;
        }
    }
}