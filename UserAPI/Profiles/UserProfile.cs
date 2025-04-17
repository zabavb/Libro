using AutoMapper;
using Library.DTOs.UserRelated.User;
using UserAPI.Models;
using UserAPI.Models.Subscription;

namespace UserAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserWithSubscriptionsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Subscriptions, opt => opt.MapFrom(src =>
                    src.UserSubscriptions != null
                        ? src.UserSubscriptions.Select(us => us.Subscription)
                        : new List<Subscription>()));

            CreateMap<Dto, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));
        }
    }
}