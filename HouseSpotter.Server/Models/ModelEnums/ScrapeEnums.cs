/// <summary>
/// Represents the type of a scrape operation.
/// </summary>
public enum ScrapeType
{
    /// <summary>
    /// Represents a recent scrape operation.
    /// </summary>
    Recent,
    /// <summary>
    /// Represents a full scrape operation.
    /// </summary>
    Full,
    /// <summary>
    /// Represents a detailed scrape operation.
    /// </summary>
    Detailed
}
/// <summary>
/// Represents the status of a scrape operation.
/// </summary>
public enum ScrapeStatus
{
    /// <summary>
    /// Represents a successful scrape operation.
    /// </summary>
    Success,
    /// <summary>
    /// Represents an ongoing scrape operation.
    /// </summary>
    Ongoing,
    /// <summary>
    /// Represents a failed scrape operation.
    /// </summary>
    Failed
}

/// <summary>
/// Represents the sites that can be scraped.
/// </summary>
public enum ScrapedSite
{
    /// <summary>
    /// Represents the Aruodas site.
    /// </summary>
    Aruodas,
    /// <summary>
    /// Represents the Domo site.
    /// </summary>
    Domo,
    /// <summary>
    /// Represents the Skelbiu site.
    /// </summary>
    Skelbiu,
    /// <summary>
    /// Represents the Facebook site.
    /// </summary>
    Facebook
}