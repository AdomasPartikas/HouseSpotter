using AutoMapper;
using HouseSpotter.Server.Context;
using HouseSpotter.Server.Models;
using HouseSpotter.Server.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseSpotter.Server.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("housespotter/db")]
    public class DatabaseController(HousingContext housingContext, IMapper mapper) : ControllerBase
    {
        private readonly HousingContext _housingContext = housingContext;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Gets all housing data from the database.
        /// </summary>
        /// <returns>The list of housing data.</returns>
        [HttpGet("getallhousing")]
        [ProducesResponseType<Housing>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllHousing()
        {
            try
            {
                var result = await _housingContext.Housings.ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        /// <summary>
        /// Gets all housing data from the database.
        /// </summary>
        /// <returns>The list of housing data.</returns>
        [HttpGet("getallusers")]
        [ProducesResponseType<List<User>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _housingContext.Users.Include(u => u.SavedSearches).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        /// <summary>
        /// Logs in a user with the provided credentials.
        /// </summary>
        /// <param name="user">The user login information.</param>
        /// <returns>The logged in user.</returns>
        [HttpPost("user/login")]
        [ProducesResponseType<User>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginBody user)
        {
            try
            {
                var result = await _housingContext.Users.Where(u => u.Username == user.Username && u.Password == user.Password).FirstOrDefaultAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        /// <summary>
        /// Registers a new user with the provided information.
        /// </summary>
        /// <param name="body">The user registration information.</param>
        /// <returns>The registered user.</returns>
        [HttpPost("user/register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterBody body)
        {
            try
            {
                var user = new User
                {
                    Username = body.Username,
                    Password = body.Password,
                    Email = body.Email,
                    PhoneNumber = body.PhoneNumber,
                    CreatedAt = DateTime.Now,
                    SavedSearches = new List<SavedSearch>(),
                    IsAdmin = body.IsAdmin
                };

                var result = await _housingContext.Users.AddAsync(user);
                await _housingContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        /// <summary>
        /// Gets the saved searches of a user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The list of saved searches.</returns>
        [HttpGet("user/{id}/savedSearches")]
        [ProducesResponseType<List<Housing>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSavedSearches(int id)
        {
            try
            {
                var user = await _housingContext.Users.Where(u => u.ID == id).Include(u => u.SavedSearches).FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound("Given user ID does not exist.");
                }
                if(user.SavedSearches == null)
                {
                    return Ok(new List<SavedSearch>());
                }

                var housingIds = user.SavedSearches.Select(ss => ss.HousingID).ToList();

                // Use the list of IDs to filter Housings
                var result = await _housingContext.Housings
                    .Where(h => housingIds.Contains(h.ID))
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        /// <summary>
        /// Saves a search for a user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="housingID">The search to save.</param>
        /// <returns>The result of the save operation.</returns>
        [HttpPut("user/{id}/saveSearch/{housingID}")]
        [ProducesResponseType<User>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SaveSearch(int id, int housingID)
        {
            try
            {
                var user = await _housingContext.Users.Where(u => u.ID == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound("Given user ID does not exist.");
                }

                if(user.SavedSearches == null)
                {
                    user.SavedSearches = new List<SavedSearch>();
                }

                if(user.SavedSearches.Any(c => c.HousingID == housingID))
                {
                    return Ok(user);
                }

                var housing = await _housingContext.Housings.Where(h => h.ID == housingID).FirstOrDefaultAsync();

                if(housing == null)
                {
                    return NotFound("Given housing ID does not exist.");
                }

                user.SavedSearches.Add(new SavedSearch { HousingID = housingID });

                await _housingContext.SaveChangesAsync();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}