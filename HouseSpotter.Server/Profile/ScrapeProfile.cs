using AutoMapper;
using HouseSpotter.Server.Models;
using HouseSpotter.Server.Models.DTO;

namespace HouseSpotterInReact.Server.Profile
{
    /// <summary>
    /// Represents a profile for mapping between ScrapeDTO and Scrape objects.
    /// </summary>
    public class ScrapeProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrapeProfile"/> class.
        /// </summary>
        public ScrapeProfile()
        {
            CreateMap<ScrapeDTO, Scrape>();
            CreateMap<Scrape, ScrapeDTO>()
                .ForMember(dto => dto.ScrapeType, opt => opt.MapFrom(src => src.ScrapeType.ToString()))
                .ForMember(dto => dto.ScrapeStatus, opt => opt.MapFrom(src => src.ScrapeStatus.ToString()))
                .ForMember(dto => dto.ScrapedSite, opt => opt.MapFrom(src => src.ScrapedSite.ToString()));
        }
    }
}