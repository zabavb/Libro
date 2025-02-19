using AutoMapper;
using Client.Models.OrderEntities.Order;
using Library.OrderEntities;

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
