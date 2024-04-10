using AutoMapper;
using HouseSpotter.Server.Models;
using HouseSpotter.Server.Models.DTO;

namespace HouseSpotter.Server.Profile
{
    /// <summary>
    /// Represents a profile for mapping between UserDTO and User objects.
    /// </summary>
    public class UserProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfile"/> class.
        /// </summary>
        public UserProfile()
        {
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>()
                .ForMember(dto => dto.SavedSearches, opt => opt.MapFrom(src => src.SavedSearches != null ? src.SavedSearches.Select(s => s.HousingID.ToString()).ToList() : new List<string>()));
        }
    }
}