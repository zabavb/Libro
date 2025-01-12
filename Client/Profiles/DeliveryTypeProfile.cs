using AutoMapper;
using Client.Models.OrderEntities.DeliveryType;
using Library.DTOs.Order;

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
