using AutoMapper;
using Client.Models.OrderEntities.Order;
using Library.DTOs.Order;

namespace Client.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile() 
        {
            CreateMap<Order, ManageOrderViewModel>();
        }
    }
}
