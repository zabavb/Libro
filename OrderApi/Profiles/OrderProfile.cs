using AutoMapper;
using OrderApi.Models;

using Library.Interfaces;
namespace OrderApi.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrderId));


            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DeliveryType, opt => opt.Ignore());

            #region OrderCardDto
            CreateMap<(Order, CollectionSnippet<BookCardSnippet>, SingleSnippet<DeliveryCardSnippet>), OrderCardDto>()
                .ConstructUsing(src => new OrderCardDto
                {
                    OrderId = src.Item1.OrderId,
                    Address = src.Item1.Address,
                    City = src.Item1.City,
                    Region = src.Item1.Region,
                    DeliveryDate = src.Item1.DeliveryDate,
                    OrderDate = src.Item1.DeliveryDate,
                    FullPrice = src.Item1.DeliveryPrice + src.Item1.Price,
                    Status = src.Item1.Status,

                    Books = src.Item2,

                    Delivery = src.Item3,
                });

            CreateMap<(Order, SingleSnippet<DeliveryCardSnippet>), OrderCardDto>()
                .ConstructUsing(src => new OrderCardDto
                {
                    OrderId = src.Item1.OrderId,
                    Address = src.Item1.Address,
                    City = src.Item1.City,
                    Region = src.Item1.Region,
                    DeliveryDate = src.Item1.DeliveryDate,
                    OrderDate = src.Item1.DeliveryDate,
                    FullPrice = src.Item1.DeliveryPrice + src.Item1.Price,
                    Status = src.Item1.Status,
                    Delivery = src.Item2,
                });

            CreateMap<(Order, CollectionSnippet<BookCardSnippet>), OrderCardDto>()
                .ConstructUsing(src => new OrderCardDto
                {
                    OrderId = src.Item1.OrderId,
                    Address = src.Item1.Address,
                    City = src.Item1.City,
                    Region = src.Item1.Region,
                    DeliveryDate = src.Item1.DeliveryDate,
                    OrderDate = src.Item1.DeliveryDate,
                    FullPrice = src.Item1.DeliveryPrice + src.Item1.Price,
                    Status = src.Item1.Status,
                    Books = src.Item2,
                });

            CreateMap<CollectionSnippet<BookCardSnippet>, CollectionSnippet<BookCardSnippet>>()
                .ConstructUsing(src =>
                    new CollectionSnippet<BookCardSnippet>(src.IsFailedToFetch, src.Items.ToList()));

            CreateMap<SingleSnippet<DeliveryCardSnippet>, SingleSnippet<DeliveryCardSnippet>>()
                .ConstructUsing(src =>
                    new SingleSnippet<DeliveryCardSnippet>(src.IsFailedToFetch, src.Item));

            CreateMap<Order, OrderCardDto>()
                .ConstructUsing(order => new OrderCardDto
                {
                    OrderId = order.OrderId,
                    Address = order.Address,
                    City = order.City,
                    Region = order.Region,
                    DeliveryDate = order.DeliveryDate,
                    OrderDate = order.DeliveryDate,
                    FullPrice = order.DeliveryPrice + order.Price,
                    Status = order.Status,
                });

            #endregion
        }
    }
}
