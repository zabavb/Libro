using AutoMapper;
using Library.DTOs.Order;
using Library.DTOs.UserRelated.User;

namespace APIComposer.GraphQL
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<(UserDto user, OrderForUserCard order), UserCard>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.user.Id))
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.user.LastName} {src.user.FirstName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.user.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.user.PhoneNumber))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.user.Role))
                .ForPath(dest => dest.Order.OrdersCount, opt => opt.MapFrom(src => src.order.OrdersCount))
                .ForPath(dest => dest.Order.LastOrder, opt => opt.MapFrom(src => src.order.LastOrder));

            CreateMap<(UserWithSubscriptionsDto user, ICollection<OrderForUserDetails> orders,
                    ICollection<FeedbackForUserDetails> feedbacks), UserDetails>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.user.Id))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.user.LastName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.user.FirstName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.user.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.user.PhoneNumber))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.user.DateOfBirth))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.user.Role))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.user.ImageUrl))
                .ForPath(dest => dest.Orders, opt => opt.MapFrom(src => src.orders))
                .ForPath(dest => dest.Feedbacks, opt => opt.MapFrom(src => src.feedbacks))
                .ForPath(dest => dest.Subscriptions, opt => opt.MapFrom(src => src.user.Subscriptions));

            CreateMap<(Order order, BookOrderDetails bookDetails), OrderDetails>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.order.Id))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.order.Status))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.order.OrderDate))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.order.Price + src.order.DeliveryPrice))
                .ForMember(dest => dest.OrderBooks, opt => opt.MapFrom(src => src.bookDetails));
        }
    }
}