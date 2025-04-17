using AutoMapper;
using Library.DTOs.UserRelated.Subscription;
using Library.DTOs.UserRelated.User;
using UserAPI.Models.Subscription;

namespace UserAPI.Profiles
{
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<SubscriptionDto, Subscription>()
                .ForMember(dst => dst.SubscriptionId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.SubscriptionId));
            
            CreateMap<Subscription, SubscriptionForUserDetails>();
        }
    }
}