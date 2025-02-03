using AutoMapper;
using DeadAspClient.Models.OrderEntities.DeliveryType;
using Library.DTOs.Order;

namespace DeadAspClient.Profiles
{
    public class DeliveryTypeProfile : Profile
    {
        public DeliveryTypeProfile()
        {
            CreateMap<DeliveryType, ManageDeliveryTypeViewModel>();
        }
    }
}
