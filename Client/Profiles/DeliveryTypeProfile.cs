using AutoMapper;
using Client.Models.OrderEntities.DeliveryType;
using Library.OrderEntities;

namespace Client.Profiles
{
    public class DeliveryTypeProfile : Profile
    {
        public DeliveryTypeProfile()
        {
            CreateMap<DeliveryType, ManageDeliveryTypeViewModel>();
        }
    }
}
