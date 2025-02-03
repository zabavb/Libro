using AutoMapper;
using DeadAspClient.Models.OrderEntities.Order;
using Library.DTOs.Order;

namespace DeadAspClient.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile() 
        {
            CreateMap<Order, ManageOrderViewModel>();
        }
    }
}
