using PuppeteerSharp;

namespace HouseSpotter.Utils
{
    public class NetworkPuppeteerClient
    {
        public async Task Destroy()
        {
            if (PuppeteerPage != null)
            {
                await PuppeteerPage.CloseAsync();
                await PuppeteerPage.DisposeAsync();
            }
            if (PuppeeteerBrowser != null)
            {
                await PuppeeteerBrowser.CloseAsync();
                await PuppeeteerBrowser.DisposeAsync();
            }
            PuppeteerInitialized = false;
        }
        public async Task Initialize()
        {
            await new BrowserFetcher().DownloadAsync();

            var launchOptions = new LaunchOptions
            {
                Headless = true
            };

            PuppeeteerBrowser = await Puppeteer.LaunchAsync(launchOptions);
            PuppeteerPage = await PuppeeteerBrowser.NewPageAsync();

            await PuppeteerPage.SetJavaScriptEnabledAsync(true);
            await PuppeteerPage.SetUserAgentAsync(UserAgents.GetRandomUserAgent());

            PuppeteerInitialized = true;
        }
        public bool PuppeteerInitialized = false;
        public IBrowser? PuppeeteerBrowser;
        public IPage? PuppeteerPage;
    }
}