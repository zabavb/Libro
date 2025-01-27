using AutoMapper;
using OrderApi.Models;

namespace OrderApi.Profiles
{
    public class DeliveryTypeProfile : Profile
    {
        public DeliveryTypeProfile()
        {
            CreateMap<DeliveryType, DeliveryTypeDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DeliveryId))
               .ReverseMap()
               .ForMember(dest => dest.DeliveryId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
