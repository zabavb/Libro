using AutoMapper;
using Library.DTOs.User;
using UserAPI.Models;

namespace UserAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.UserId))
                .ReverseMap()
                .ForMember(dst => dst.UserId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
