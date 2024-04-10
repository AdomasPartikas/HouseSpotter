namespace HouseSpotter.Server.Models.DTO
{
    /// <summary>
    /// Represents the request body for user login.
    /// </summary>
    public class UserLoginBody
    {
        /// <summary>
        /// Gets or sets the username for login.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password for login.
        /// </summary>
        public required string Password { get; set; }
    }

    /// <summary>
    /// Represents the request body for user registration.
    /// </summary>
    public class UserRegisterBody
    {
        /// <summary>
        /// Gets or sets the username for registration.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the password for registration.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the email for registration.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number for registration.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is an admin during registration.
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}