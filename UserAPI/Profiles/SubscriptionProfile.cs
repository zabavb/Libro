﻿using AutoMapper;
using UserAPI.Models;

namespace UserAPI.Profiles
{
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile() {
            CreateMap<Subscription, SubscriptionDto>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.SubscriptionId))
                .ReverseMap()
                .ForMember(dst => dst.SubscriptionId, opt => opt.Ignore());
        }
    }
}