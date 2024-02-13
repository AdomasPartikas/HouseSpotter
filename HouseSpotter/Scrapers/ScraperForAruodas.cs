using HtmlAgilityPack;
using HouseSpotter.Utils;
using System.Diagnostics;
using HouseSpotter.Models;

namespace HouseSpotter.Scrapers
{
    public class ScraperForAruodas
    {
        private Random random = new Random();
        private ScraperClient _scrapperClient;
        public ScraperForAruodas(ScraperClient scraperClient)
        {
            _scrapperClient = scraperClient;
        }
        public async Task FindApartaments(string siteEndpoint)
        {
            Debug.WriteLine($"[{DateTimeOffset.Now}] Running FindApartaments for Aruodas with {siteEndpoint} endpoint");

            string url = $"https://m.aruodas.lt/{siteEndpoint}/";
            string pageUrl = url;
            int pageCount = 1;

            for(int i = 1; i <= pageCount; i++)
            {
                Thread.Sleep(_scrapperClient.SpeedLimit); //Stopping to not get flagged as a robot

                if(i > 1)
                    pageUrl = url + $"puslapis/{i}/";

                var response = await _scrapperClient.Client.GetAsync(pageUrl);
                var body = await response.Content.ReadAsStringAsync();

                var html = await _scrapperClient.Client.GetStringAsync(pageUrl);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                if (pageCount == 1)
                {
                    var pageSelector = doc.DocumentNode.Descendants("a")
                        .Where(node => node.GetAttributeValue("href", "")
                        .Contains("Page&advertCount")).FirstOrDefault()!.InnerText;

                    pageCount = Convert.ToInt32(pageSelector.Remove(0, 21).Trim());
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
                    }
                }
            }
        }
        public void SaveResult(bool savingDto, object result)
        {
            if(savingDto)
            {
                var house = result as HouseDTO;
                //Something something something.......
            }
            else
            {
                var text = result as String;
                if(_scrapperClient.UsingSql)
                {
                    //EstablishSqlConnection() turbut
                    //Something something something.....
                    //DestroySqlConnection() turbut
                }
                else
                {
                    Debug.WriteLine($"[{DateTimeOffset.Now}] Link scrapped: {text}");
                }
            }
        }
    }
}