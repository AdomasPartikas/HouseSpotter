using AutoMapper;
using HouseSpotter.Server.Models.DTO;
using HouseSpotter.Server.Scrapers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseSpotter.Server.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("housespotter/scrapers")]
    public class ScraperController(ScraperForSkelbiu scraperForSkelbiu, IMapper mapper) : ControllerBase
    {
        private readonly ScraperForSkelbiu _scraperForSkelbiu = scraperForSkelbiu;
        private readonly IMapper _mapper = mapper;

        [Authorize]
        [HttpPost("skelbiu/housing/{depth}")]
        [ProducesResponseType<ScrapeDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SkelbiuScrapeHousing(int depth)
        {
            try
            {
                var result = await _scraperForSkelbiu.FindHousingPosts(depth);

                return Ok(_mapper.Map<ScrapeDTO>(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}