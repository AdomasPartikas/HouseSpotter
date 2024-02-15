using HtmlAgilityPack;
using HouseSpotter.Utils;
using System.Diagnostics;
using HouseSpotter.Models;
using System.Text.RegularExpressions;

namespace HouseSpotter.Scrapers
{
    public class ScraperForAruodas
    {
        private Random random = new Random();
        private ScraperClient _scrapperClient;
        private ILogger<ScraperForAruodas> _logger;
        public ScraperForAruodas(ScraperClient scraperClient, ILogger<ScraperForAruodas> logger)
        {
            _scrapperClient = scraperClient;
            _logger = logger;
        }
        ~ScraperForAruodas()
        {
            _scrapperClient.EndScrape().Wait();
        }
        public async Task GetHousingDetails(string url)
        {
            Thread.Sleep((int)_scrapperClient.SpeedLimit!); //Stopping to not get flagged as a robot

            string html = "";
            var doc = new HtmlDocument();

            var house = new HouseDTO();

            try
            {
                Debug.WriteLine($"[{DateTimeOffset.Now}] Trying HtmlClient");

                if(!_scrapperClient.NetworkHttpClient.HtmlClientInitialized)
                {
                    await _scrapperClient.NetworkHttpClient.Initialize();
                }
                html = await _scrapperClient.NetworkHttpClient.HtmlClient.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine($"[{DateTimeOffset.Now}] HtmlClient failed");

                try
                {
                    Debug.WriteLine($"[{DateTimeOffset.Now}] Trying PuppeteerSharp");

                    if (!_scrapperClient.NetworkPuppeteerClient.PuppeteerInitialized)
                    {
                        await _scrapperClient.NetworkPuppeteerClient.Initialize();
                        await _scrapperClient.NetworkPuppeteerClient.PuppeteerPage!.GetCookiesAsync("https://m.aruodas.lt/");
                        Thread.Sleep(125);
                    }
                    
                    await _scrapperClient.NetworkPuppeteerClient.PuppeteerPage!.GoToAsync(url);
                    html = await _scrapperClient.NetworkPuppeteerClient.PuppeteerPage!.GetContentAsync();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine($"[{DateTimeOffset.Now}] PuppeteerSharp failed");
                    _logger.LogCritical(ex, $"[{DateTimeOffset.Now}] Both HtmlClient and PuppeteerSharp failed to get {url}");
                    return;
                }
            }

            doc.LoadHtml(html);

            var header = doc.DocumentNode.Descendants("div")
                   .Where(node => node.GetAttributeValue("class", "")
                    .Equals("advert-info-header")).ToList();
            
            var title = header[1].Descendants("h1").FirstOrDefault()!.InnerText.Trim();
            var price = Convert.ToDouble(header[1].Descendants("span").FirstOrDefault()!.InnerText.TrimEnd('€').Trim().Replace(" ", ""));

            house.Link = url;
            house.Title = title;
            house.Kaina = price;
            
        }
        public async Task FindHousing(string siteEndpoint)
        {
            Debug.WriteLine($"[{DateTimeOffset.Now}] Running FindApartaments for Aruodas with {siteEndpoint} endpoint");

            string url = $"https://m.aruodas.lt/{siteEndpoint}/";
            string pageUrl = url;
            int pageCount = 1;

            for (int i = 1; i <= pageCount; i++)
            {
                Thread.Sleep((int)_scrapperClient.SpeedLimit!); //Stopping to not get flagged as a robot

                if (i > 1)
                    pageUrl = url + $"puslapis/{i}/";

                string html = "";
                var doc = new HtmlDocument();

                try
                {
                    Debug.WriteLine($"[{DateTimeOffset.Now}] Trying HtmlClient");

                    if(!_scrapperClient.NetworkHttpClient.HtmlClientInitialized)
                    {
                        await _scrapperClient.NetworkHttpClient.Initialize();
                    }
                    html = await _scrapperClient.NetworkHttpClient.HtmlClient.GetStringAsync(pageUrl);
                }
                catch (HttpRequestException e)
                {
                    Debug.WriteLine($"[{DateTimeOffset.Now}] HtmlClient failed");

                    try
                    {
                        Debug.WriteLine($"[{DateTimeOffset.Now}] Trying PuppeteerSharp");

                        if (!_scrapperClient.NetworkPuppeteerClient.PuppeteerInitialized)
                        {
                            await _scrapperClient.NetworkPuppeteerClient.Initialize();
                            await _scrapperClient.NetworkPuppeteerClient.PuppeteerPage!.GetCookiesAsync("https://m.aruodas.lt/");
                            Thread.Sleep(125);
                        }
                        
                        await _scrapperClient.NetworkPuppeteerClient.PuppeteerPage!.GoToAsync(pageUrl);
                        html = await _scrapperClient.NetworkPuppeteerClient.PuppeteerPage!.GetContentAsync();
                    }
                    catch(Exception ex)
                    {
                        Debug.WriteLine($"[{DateTimeOffset.Now}] PuppeteerSharp failed");
                        _logger.LogCritical(ex, $"[{DateTimeOffset.Now}] Both HtmlClient and PuppeteerSharp failed to get {pageUrl}");
                        return;
                    }
                }

                doc = new HtmlDocument();
                doc.LoadHtml(html);

                if (pageCount == 1)
                {
                    //    For www.aruodas.lt
                    // var pageSelector = doc.DocumentNode.Descendants("a")
                    //     .Where(node => node.GetAttributeValue("class", "")
                    //     .Equals("page-bt")).ToList();

                    // pageCount = pageSelector.Select(item =>
                    // {
                    //     var innerText = item.InnerText.Trim();
                    //     if(Regex.IsMatch(innerText, @"^\d+$", RegexOptions.IgnoreCase))
                    //         return int.Parse(innerText);
                    //     else 
                    //         return 0;
                    // }).ToList().Max();

                    // Debug.WriteLine($"[{DateTimeOffset.Now}] Found {pageCount} pages for {siteEndpoint}");

                    var pageSelect = doc.DocumentNode.Descendants("div")
                        .Where(node => node.GetAttributeValue("class", "")
                        .Equals("page-select-v2")).FirstOrDefault();

                    var node = pageSelect!.Descendants("a").FirstOrDefault()!.InnerText;

                    pageCount = Convert.ToInt32(node.Remove(0, 21).Trim());
                }

                var abstractList = doc.DocumentNode.Descendants("ul")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("search-result-list-big_thumbs")).ToList();

                var listOfSelections = abstractList[0].Descendants("li").ToList();

                _scrapperClient.Queries += listOfSelections.Count();

                foreach (var item in listOfSelections)
                {

                    var tip = item.Descendants("a")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Equals("result-item-info-container-big_thumbs"))
                            .FirstOrDefault();

                    if (tip != null)
                    {
                        var text = "\nhttps://m.aruodas.lt/" + tip.GetAttributeValue("href", "");
                        SaveResult(false, text);
                        await GetHousingDetails(text); //For testing purposes
                    }
                }

                Debug.WriteLine($"[{DateTimeOffset.Now}] <{i}/{pageCount}> Total queries made so far: {_scrapperClient.Queries} in /{siteEndpoint}/");
                return; //For testing purposes
            }
        }
        public void SaveResult(bool savingDto, object result)
        {
            if (savingDto)
            {
                var house = result as HouseDTO;
                //Something something something.......
            }
            else
            {
                var text = result as String;
                if (_scrapperClient.UsingSql)
                {
                    //EstablishSqlConnection() turbut
                    //Something something something.....
                    //DestroySqlConnection() turbut
                }
                else
                {
                    //Debug.WriteLine($"[{DateTimeOffset.Now}] {text}");
                    _scrapperClient.Queries++;
                }
            }
        }
    }
}