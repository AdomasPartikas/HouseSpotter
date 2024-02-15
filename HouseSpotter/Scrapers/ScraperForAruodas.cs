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
        private ScraperClient _scraperClient;
        private ILogger<ScraperForAruodas> _logger;
        public ScraperForAruodas(ScraperClient scraperClient, ILogger<ScraperForAruodas> logger)
        {
            _scraperClient = scraperClient;
            _logger = logger;
        }
        ~ScraperForAruodas()
        {
            _scraperClient.EndScrape().Wait();
        }
        public async Task GetHousingDetails(string url, string bustoTipas)
        {
            Thread.Sleep((int)_scraperClient.SpeedLimit!); //Stopping to not get flagged as a robot

            string html = "";
            var doc = new HtmlDocument();

            var house = new HouseDTO();

            try
            {
                if (!_scraperClient.NetworkPuppeteerClient.PuppeteerInitialized)
                {
                    await _scraperClient.NetworkPuppeteerClient.Initialize();
                    await _scraperClient.NetworkPuppeteerClient.PuppeteerPage!.GetCookiesAsync("https://m.aruodas.lt/");
                    Thread.Sleep(125);
                }

                await _scraperClient.NetworkPuppeteerClient.PuppeteerPage!.GoToAsync(url);
                html = await _scraperClient.NetworkPuppeteerClient.PuppeteerPage!.GetContentAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"[{DateTimeOffset.Now}] PuppeteerSharp failed to get {url}");
                return;
            }

            doc.LoadHtml(html);

            var header = doc.DocumentNode.Descendants("div")
                   .Where(node => node.GetAttributeValue("class", "")
                    .Equals("advert-info-header")).ToList();

            string title = "N/A";
            double price = 0;

            if (header.Count == 1)
            {
                header = doc.DocumentNode.Descendants("div")
                   .Where(node => node.GetAttributeValue("class", "")
                    .Equals("advert-info-header  in-project ")).ToList();

                title = header[0].Descendants("h1").FirstOrDefault()!.InnerText.Trim();
                price = Convert.ToDouble(header[0].Descendants("span").FirstOrDefault()!.InnerText.TrimEnd('€').Trim().Replace(" ", ""));
            }
            else
            {
                title = header[1].Descendants("h1").FirstOrDefault()!.InnerText.Trim();
                price = Convert.ToDouble(header[1].Descendants("span").FirstOrDefault()!.InnerText.TrimEnd('€').Trim().Replace(" ", ""));
            }

            house.Link = url;
            house.Title = title;
            house.Kaina = price;
            house.BustoTipas = bustoTipas;
            house.AnketosKodas = Regex.Match(url, @"\d{6,7}").Value;
            house.Aprasymas = doc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("collapsedText")).FirstOrDefault()!.InnerText;

            var abst = doc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("object-info ")).ToList();

            if (abst.Count == 0)
                abst = doc.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("object-info with-rentdanger")).ToList();

            var descriptionList = abst[0].Descendants("dl").ToList();
            var descriptionTitle = descriptionList[0].Descendants("dt").ToList();
            var descriptionDefinition = descriptionList[0].Descendants("dd").ToList();

            for (int i = 0; i < descriptionTitle.Count; i++)
            {
                var dscTitle = descriptionTitle[i].InnerText;
                var dscDefinition = descriptionDefinition[i].InnerText;

                dscDefinition = Regex.Replace(dscDefinition, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");

                switch (dscTitle)
                {
                    case "Kaina mėn.":
                        {
                            dscDefinition = Regex.Replace(dscDefinition, @"\s+", "");
                            dscDefinition = Regex.Replace(dscDefinition, @"€", "");
                            house.KainaMen = Convert.ToInt32(dscDefinition);
                        }
                        break;
                    case "Namo numeris":
                        {
                            house.NamoNumeris = dscDefinition;
                        }
                        break;
                    case "Buto numeris":
                        {
                            house.ButoNumeris = dscDefinition;
                        }
                        break;
                    case "Kambarių sk.":
                        {
                            house.KambariuSk = Convert.ToInt32(dscDefinition);
                        }
                        break;
                    case "Plotas":
                        {
                            dscDefinition = dscDefinition.Replace('²', ' ').Replace('m', ' ').Trim();
                            house.Plotas = Convert.ToDouble(dscDefinition);
                        }
                        break;
                    case "Aukštas":
                        {
                            house.Aukstas = Convert.ToInt32(dscDefinition);
                        }
                        break;
                    case "Aukštų sk.":
                        {
                            house.AukstuSk = Convert.ToInt32(dscDefinition);
                        }
                        break;
                    case "Metai":
                        {
                            if (Regex.IsMatch(dscDefinition, @"\d{4} renovacija"))
                            {
                                dscDefinition = Regex.Match(dscDefinition, @"\d{4} renovacija").Value;
                                dscDefinition = Regex.Match(dscDefinition, @"\d{4}").Value;
                                house.Metai = Convert.ToInt32(dscDefinition);
                            }
                            else
                                house.Metai = Convert.ToInt32(dscDefinition);

                        }
                        break;
                    case "Įrengimas":
                        {
                            house.Irengimas = dscDefinition;
                        }
                        break;
                    case "Pastato tipas":
                        {
                            house.PastatoTipas = dscDefinition;
                        }
                        break;
                    case "Šildymas":
                        {
                            house.Sildymas = dscDefinition;
                        }
                        break;
                    case "Pastato energijos suvartojimo klasė":
                        {
                            house.PastatoEnergijosSuvartojimoKlase = dscDefinition;
                        }
                        break;
                    case "Ypatybės":
                        {

                            dscDefinition = String.Join('/', descriptionDefinition[i].Descendants("span")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Equals("special-comma")).Select(x => x.InnerHtml.Trim()).ToList());
                            house.Ypatybes = dscDefinition;
                        }
                        break;
                    case "Papildomos patalpos":
                        {
                            dscDefinition = String.Join('/', descriptionDefinition[i].Descendants("span")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Equals("special-comma")).Select(x => x.InnerHtml.Trim()).ToList());
                            house.PapildomosPatalpos = dscDefinition;
                        }
                        break;
                    case "Papildoma įranga":
                        {
                            dscDefinition = String.Join('/', descriptionDefinition[i].Descendants("span")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Equals("special-comma")).Select(x => x.InnerHtml.Trim()).ToList());
                            house.PapildomaIranga = dscDefinition;
                        }
                        break;
                    case "Apsauga":
                        {
                            dscDefinition = String.Join('/', descriptionDefinition[i].Descendants("span")
                                .Where(node => node.GetAttributeValue("class", "")
                                .Equals("special-comma")).Select(x => x.InnerHtml.Trim()).ToList());
                            house.Apsauga = dscDefinition;
                        }
                        break;
                    case "Vanduo":
                        {
                            house.Vanduo = dscDefinition;
                        }
                        break;
                    case "Iki vandens telkinio (m)":
                        {
                            house.IkiTelkinio = Convert.ToInt32(dscDefinition.Replace("m", "").Replace(" ", ""));
                        }
                        break;
                    case "Unikalus daikto numeris (RC numeris)":
                        {
                            house.RCNumeris = dscDefinition;
                        }
                        break;
                    case "Namo tipas":
                        {
                            house.NamoTipas = dscDefinition;
                        }
                        break;
                    case "Sklypo plotas":
                        {
                            house.SklypoPlotas = dscDefinition;
                        }
                        break;
                    case "Artimiausias vandens telkinys":
                        {
                            house.ArtimiausiasTelkinys = dscDefinition;
                        }
                        break;
                    default:
                        {
                            Debug.WriteLine($"Title not defined: {dscTitle}");
                            Thread.Sleep(1000);
                        }
                        break;
                }
            }

            Debug.WriteLine($"[{DateTimeOffset.Now}] {house.Title} - {house.Kaina}€");
            Debug.WriteLine($"Anketos Kodas: {house.AnketosKodas}");
            Debug.WriteLine($"Link'as: {house.Link}");
            Debug.WriteLine($"Busto tipas: {house.BustoTipas}");
            Debug.WriteLine($"Kaina: {house.Kaina.ToString()}");
            Debug.WriteLine($"Kaina men.: {house.KainaMen.ToString()}");
            Debug.WriteLine($"Namo numeris: {house.NamoNumeris}");
            Debug.WriteLine($"Buto numeris: {house.ButoNumeris}");
            Debug.WriteLine($"Kambariu sk: {house.KambariuSk.ToString()}");
            Debug.WriteLine($"Plotas: {house.Plotas.ToString()}");
            Debug.WriteLine($"Aukstas: {house.Aukstas.ToString()}");
            Debug.WriteLine($"Aukstu sk.: {house.AukstuSk.ToString()}");
            Debug.WriteLine($"Metai: {house.Metai.ToString()}");
            Debug.WriteLine($"Irengimas: {house.Irengimas}");
            Debug.WriteLine($"Pastato tipas: {house.PastatoTipas}");
            Debug.WriteLine($"Sildymas: {house.Sildymas}");
            Debug.WriteLine($"Energijos klase: {house.PastatoEnergijosSuvartojimoKlase}");
            Debug.WriteLine($"Ypatybes: {house.Ypatybes}");
            Debug.WriteLine($"Papildomos patalpos: {house.PapildomosPatalpos}");
            Debug.WriteLine($"Papildoma iranga: {house.PapildomaIranga}");
            Debug.WriteLine($"Apsauga: {house.Apsauga}");
        }
        public async Task FindHousing(string siteEndpoint)
        {
            Debug.WriteLine($"[{DateTimeOffset.Now}] Running FindApartaments for Aruodas with {siteEndpoint} endpoint");

            string url = $"https://m.aruodas.lt/{siteEndpoint}/";
            string pageUrl = url;
            int pageCount = 1;

            for (int i = 1; i <= pageCount; i++)
            {
                Thread.Sleep((int)_scraperClient.SpeedLimit!); //Stopping to not get flagged as a robot

                if (i > 1)
                    pageUrl = url + $"puslapis/{i}/";

                string html = "";
                var doc = new HtmlDocument();

                try
                {
                    if (!_scraperClient.NetworkPuppeteerClient.PuppeteerInitialized)
                    {
                        await _scraperClient.NetworkPuppeteerClient.Initialize();
                        await _scraperClient.NetworkPuppeteerClient.PuppeteerPage!.GetCookiesAsync("https://m.aruodas.lt/");
                        Thread.Sleep(125);
                    }

                    await _scraperClient.NetworkPuppeteerClient.PuppeteerPage!.GoToAsync(pageUrl);
                    html = await _scraperClient.NetworkPuppeteerClient.PuppeteerPage!.GetContentAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, $"[{DateTimeOffset.Now}]PuppeteerSharp failed to get {pageUrl}");
                    return;
                }

                doc = new HtmlDocument();
                doc.LoadHtml(html);

                if (pageCount == 1)
                {
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

                foreach (var item in listOfSelections)
                {

                    var tip = item.Descendants("a")
                            .Where(node => node.GetAttributeValue("class", "")
                            .Equals("result-item-info-container-big_thumbs"))
                            .FirstOrDefault();

                    if (tip != null)
                    {
                        var text = "https://m.aruodas.lt/" + tip.GetAttributeValue("href", "");

                        try
                        {
                            SaveResult(false, text);
                            await GetHousingDetails(text, siteEndpoint); //For testing purposes
                            _scraperClient.Queries++;
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, $"[{DateTimeOffset.Now}] Error while getting details for {text}");
                        }
                    }
                }

                Debug.WriteLine($"[{DateTimeOffset.Now}] <{i}/{pageCount}> Total queries made so far: {_scraperClient.Queries} in /{siteEndpoint}/");
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
                if (_scraperClient.UsingSql)
                {
                    //EstablishSqlConnection() turbut
                    //Something something something.....
                    //DestroySqlConnection() turbut
                }
                else
                {
                    //Debug.WriteLine($"[{DateTimeOffset.Now}] {text}");
                }
            }
        }
    }
}