using System;

namespace HouseSpotter.Server.Models.DTO
{
    /// <summary>
    /// Represents a data transfer object for a scrape operation.
    /// </summary>
    public class ScrapeDTO
    {
        /// <summary>
        /// Gets or sets the ID of the scrape.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the result of the scrape.
        /// </summary>
        public string? ScrapeResult { get; set; }

        /// <summary>
        /// Gets or sets the type of the scrape.
        /// </summary>
        public string? ScrapeType { get; set; }

        /// <summary>
        /// Gets or sets the status of the scrape.
        /// </summary>
        public string? ScrapeStatus { get; set; }

        /// <summary>
        /// Gets or sets the site that was scraped.
        /// </summary>
        public string? ScrapedSite { get; set; }

        /// <summary>
        /// Gets or sets the date when the scrape was performed.
        /// </summary>
        public DateTime DateScraped { get; set; }

        /// <summary>
        /// Gets or sets the duration of the scrape operation.
        /// </summary>
        public TimeSpan ScrapeTime { get; set; }

        /// <summary>
        /// Gets or sets the message associated with the scrape.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the total number of queries performed during the scrape.
        /// </summary>
        public int? TotalQueries { get; set; }

        /// <summary>
        /// Gets or sets the number of new queries performed during the scrape.
        /// </summary>
        public int? NewQueries { get; set; }

        /// <summary>
        /// Gets or sets the number of errors that occurred during the scrape.
        /// </summary>
        public int? TotalErrors { get; set; }

        /// <summary>
        /// Gets or sets the error messages that occurred during the scrape.
        /// </summary>
        public List<string>? ErrorMessages { get; set; }
    }
}