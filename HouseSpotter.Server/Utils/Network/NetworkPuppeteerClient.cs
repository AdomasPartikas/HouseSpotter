using PuppeteerSharp;

namespace HouseSpotter.Server.Utils
{
    /// <summary>
    /// Represents a client for interacting with Puppeteer network automation library.
    /// </summary>
    public class NetworkPuppeteerClient
    {
        /// <summary>
        /// Destroys the Puppeteer client by closing and disposing the browser and page instances.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Initializes the Puppeteer client by downloading the browser, launching it, and creating a new page instance.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public virtual async Task Initialize()
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

        /// <summary>
        /// Gets or sets a value indicating whether the Puppeteer client has been initialized.
        /// </summary>
        public virtual bool PuppeteerInitialized { get; set; } = false;

        /// <summary>
        /// Gets or sets the Puppeteer browser instance.
        /// </summary>
        public virtual IBrowser? PuppeeteerBrowser {get; set;}

        /// <summary>
        /// Gets or sets the Puppeteer page instance.
        /// </summary>
        public virtual IPage? PuppeteerPage {get; set;}
    }
}