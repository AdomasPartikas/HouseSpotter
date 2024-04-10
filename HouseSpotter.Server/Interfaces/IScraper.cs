using HouseSpotter.Server.Models;

namespace HouseSpotter.Server.Interfaces
{
    public interface IScraper
    {
        Task GetHousingDetails(Housing house);
        Task<Scrape> ScrapeHousingPosts(string pageUrl, string siteEndpoint, Scrape scrape);
        Task<Scrape> FindHousingPosts(int scrapeDepth);
    }
}