using AutoMapper;
using OrderApi.Models;

namespace OrderApi.Profiles
{
    public class DeliveryTypeProfile : Profile
    {
        public DeliveryTypeProfile()
        {
            CreateMap<DeliveryType, DeliveryTypeDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DeliveryId)).ReverseMap();
        }
    }
}
