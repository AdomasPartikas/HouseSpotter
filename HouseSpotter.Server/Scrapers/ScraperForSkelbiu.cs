using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using HouseSpotter.Server.Context;
using HouseSpotter.Server.Interfaces;
using HouseSpotter.Server.Models;
using HouseSpotter.Server.Scrapers;
using HouseSpotter.Server.Utils;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

namespace HouseSpotter.Server.Scrapers
{
    public class ScraperForSkelbiu : ScraperBase
    {
        public ScraperForSkelbiu(ScraperClient _scraperClient, ILogger<ScraperForSkelbiu> _logger, HousingContext _housingContext)
            : base(_scraperClient, _logger, _housingContext) { }

        ~ScraperForSkelbiu()
        {
            _scraperClient.EndScrape().Wait();
        }
        private async Task EndScrape()
        {
            await _scraperClient.EndScrape();
        }

        public override async Task GetHousingDetails(Housing house)
        {
            await Task.Delay((int)_scraperClient.SpeedLimit!); //Stopping to not get flagged as a robot

            string html = "";
            var doc = new HtmlDocument();

            try
            {
                await EnsurePuppeteerInitialized("https://m.skelbiu.lt/");

                await _scraperClient.NetworkPuppeteerClient.PuppeteerPage!.GoToAsync(house.Link);
                html = await _scraperClient.NetworkPuppeteerClient.PuppeteerPage!.GetContentAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"[{DateTimeOffset.Now}] PuppeteerSharp failed to get {house.Link}");
                return;
            }

            doc.LoadHtml(html);

            var header = doc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("bd")).FirstOrDefault();

            var imageHolder = doc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("photoArea")).FirstOrDefault();

            house.Nuotrauka = imageHolder!.Descendants("img").FirstOrDefault()!.GetAttributeValue("src", "");

            house.Title = header!.Descendants("h1")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("item-title")).FirstOrDefault()!.InnerText;

            house.Kaina = Convert.ToDouble(header!.Descendants("div")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("price")).FirstOrDefault()!.FirstChild.InnerText.Replace(" ", "").Trim().TrimEnd('€'));

            house.AnketosKodas = Regex.Match(house.Link!, @"(\d+)\.html").Groups[1].Value;

            house.Aprasymas = doc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("description")).FirstOrDefault()!.InnerText;

            var detailsMoreArea = doc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("detailsMoreArea")).FirstOrDefault();

            var detailTextList = detailsMoreArea!.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("dataText")).ToList();

            var detailInfoList = detailsMoreArea!.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("dataInfo")).ToList();

            var features = detailsMoreArea.Descendants("div")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("dataDetailsArea")).FirstOrDefault();

            for (int i = 0; i < detailTextList.Count; i++)
            {
                var text = detailTextList[i].InnerText.Trim();
                var info = detailInfoList[i].InnerText.Trim();

                switch (text)
                {
                    case "Gyvenvietė:":
                        {
                            house.Gyvenviete = info;
                        }
                        break;
                    case "Gatvė:":
                        {
                            house.Gatve = info;
                        }
                        break;
                    case "Įrengimas:":
                        {
                            house.Irengimas = info;
                        }
                        break;
                    case "Pastato tipas:":
                        {
                            house.NamoTipas = info;
                        }
                        break;
                    case "Tipas:":
                        {
                            house.PastatoTipas = info;
                        }
                        break;
                    case "Metai:":
                        {
                            house.Metai = Convert.ToInt32(Regex.Match(info, @"(\d{4})").Groups[1].Value);
                        }
                        break;
                    case "Plotas, m²:":
                        {
                            house.Plotas = Convert.ToDouble(info.Replace('²', ' ').Replace('m', ' ').Trim());
                        }
                        break;
                    case "Aukštas:":
                        {
                            house.Aukstas = Convert.ToInt32(info);
                        }
                        break;
                    case "Aukštų skaičius:":
                        {
                            house.AukstuSk = Convert.ToInt32(info);
                        }
                        break;
                    case "Kamb. sk.:":
                        {
                            house.KambariuSk = Convert.ToInt32(info);
                        }
                        break;
                    case "Sklypo plotas, a:":
                        {
                            house.SklypoPlotas = info;
                        }
                        break;
                    case "Šildymas:":
                        {
                            house.Sildymas = info;
                        }
                        break;
                    case "Namo numeris:":
                        {
                            house.NamoNumeris = info;
                        }
                        break;
                    case "Energetinė klasė:":
                        {
                            house.PastatoEnergijosSuvartojimoKlase = info;
                        }
                        break;
                    default:
                        {
                            _logger.LogWarning($"[{DateTimeOffset.Now}] Unhandled detail: {text}");
                        }
                        break;
                }
            }

            if (features != null)
            {
                var featuresList = features.InnerText.Split(", ");
                house.Ypatybes = string.Join("/", featuresList);
            }
        }

        private async Task<int> DeterminePageCount(string baseUrl)
        {
            try
            {
                await EnsurePuppeteerInitialized("https://m.skelbiu.lt/");
                await _scraperClient.NetworkPuppeteerClient.PuppeteerPage!.GoToAsync(baseUrl);
                string html = await _scraperClient.NetworkPuppeteerClient.PuppeteerPage.GetContentAsync();

                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var pagingLink = doc.DocumentNode.Descendants("a")
                                    .Where(node => node.GetAttributeValue("id", "")
                                    .Equals("pagingLink")).FirstOrDefault()?.InnerText;

                if (!string.IsNullOrEmpty(pagingLink))
                {
                    var match = Regex.Match(pagingLink, @"1 - (\d+) iš (\d+)");
                    if (match.Success)
                    {
                        double totalItems = double.Parse(match.Groups[2].Value);
                        double itemsPerPage = double.Parse(match.Groups[1].Value);
                        return (int)Math.Ceiling(totalItems / itemsPerPage);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to determine page count for {baseUrl}");
            }
            return 1; // Default to a single page if unable to determine
        }

        public override async Task<Scrape> FindHousingPosts(int scrapeDepth)
        {
            var scrape = await InitializeScrape(scrapeDepth != -1 ? ScrapeType.Partial : ScrapeType.Full, ScrapedSite.Skelbiu);

            for (int i = 0; i < 2; i++) // Loop through site endpoints
            {
                string siteEndpoint = i switch
                {
                    0 => "namai",
                    1 => "butai",
                    _ => throw new InvalidOperationException("Invalid endpoint")
                };

                if(scrape.ScrapeStatus == ScrapeStatus.Completed)
                    scrape.ScrapeStatus = ScrapeStatus.Ongoing;

                _logger.LogInformation($"[{DateTimeOffset.Now}] Running scraper for Skelbiu with {siteEndpoint} endpoint.");

                string baseUrl = $"https://m.skelbiu.lt/skelbimai/nekilnojamasis-turtas/{siteEndpoint}/";
                string pageUrl = baseUrl;

                int pageCount = await DeterminePageCount(baseUrl);
                int pagesToScrape = scrapeDepth == -1 ? pageCount : Math.Min(scrapeDepth, pageCount);

                for (int page = 1; page <= pagesToScrape; page++)
                {
                    pageUrl = page > 1 ? $"{baseUrl}{page}" : baseUrl;
                    await Task.Delay((int)_scraperClient.SpeedLimit!);

                    try
                    {
                        await EnsurePuppeteerInitialized("https://m.skelbiu.lt/");
                        await ScrapeHousingPosts(pageUrl, siteEndpoint, scrape);

                        scrape = await ScrapeUpdate(scrape);

                        if(scrape.ScrapeStatus == ScrapeStatus.Completed)
                            break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical(ex, $"Failed to scrape {pageUrl}");

                        scrape.TotalErrors++;
                        scrape.Errors ??= new List<ScrapeError>();
                        scrape.Errors.Add(new ScrapeError
                        {
                            Message = $"Critical failure on page {pageUrl}",
                            StackTrace = ex.StackTrace
                        });
                    }
                }
            }

            var result = await CompleteScrape(scrape, ScrapeResult.Success, "Scrape finished successfully");

            await EndScrape();

            return result;
        }

        public override async Task<Scrape> ScrapeHousingPosts(string pageUrl, string siteEndpoint, Scrape scrape)
        {
            try
            {
                await _scraperClient.NetworkPuppeteerClient.PuppeteerPage!.GoToAsync(pageUrl);
                string htmlContent = await _scraperClient.NetworkPuppeteerClient.PuppeteerPage.GetContentAsync();

                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                var housingPosts = ExtractHousingPosts(doc);

                foreach (var post in housingPosts)
                {
                    var existingHousing = await _housingContext.Housings.FirstOrDefaultAsync(h => h.Link == post);

                    if(existingHousing == null)
                    {
                        _scraperClient.NewQueries++;

                        var housing = new Housing
                        {
                            Link = post,
                            BustoTipas = siteEndpoint,
                            ScrapedSite = scrape.ScrapedSite
                        };

                        _housingContext.Housings.Add(housing);

                        await GetHousingDetails(housing);

                        await _housingContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to scrape housing posts from {pageUrl}.");
                scrape.TotalErrors++;
                scrape.Errors ??= new List<ScrapeError>();
                scrape.Errors.Add(new ScrapeError
                {
                    Message = $"Failed to scrape housing post from {pageUrl}.",
                    StackTrace = ex.StackTrace
                });
            }

            return scrape;
        }

        private List<string> ExtractHousingPosts(HtmlDocument doc)
        {
            var housingPosts = new List<string>();

            var listItems = doc.DocumentNode
                        .Descendants("div")
                        .Where(node => node.GetAttributeValue("class", "")
                        .Equals("gallery-list")).FirstOrDefault()!
                        .Descendants("div")
                        .Where(node => node.GetAttributeValue("class", "")
                        .Equals("item")).ToList();

            foreach (var post in listItems)
            {
                var tip = post.Descendants("a")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Equals("gallery-item-element-link js-cfuser-link"))
                            .FirstOrDefault();

                if (tip != null)
                {
                    var text = "https://m.skelbiu.lt" + tip.GetAttributeValue("href", "");

                    _scraperClient.TotalQueries++;

                    housingPosts.Add(text);
                }
            }

            return housingPosts;
        }


    }
}