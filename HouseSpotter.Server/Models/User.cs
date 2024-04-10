namespace HouseSpotter.Server.Models
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the ID of the user.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the user.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the list of saved searches for the user.
        /// </summary>
        public List<SavedSearch>? SavedSearches { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is an admin.
        /// </summary>
        public bool IsAdmin { get; set; }
    }

    /// <summary>
    /// Represents a saved search for a user.
    /// </summary>
    public class SavedSearch
    {
        /// <summary>
        /// Gets or sets the ID of the saved search.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Gets or sets the search query for the saved search.
        /// </summary>
        public int HousingID { get; set; }
    }
}