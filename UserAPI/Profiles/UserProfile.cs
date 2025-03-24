using AutoMapper;
using Library.DTOs.UserRelated.User;
using UserAPI.Models;

namespace UserAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<(User, OrderCardSnippet), CardDto>()
                .ConstructUsing(src => new CardDto
                {
                    UserId = src.Item1.UserId,
                    FullName = $"{src.Item1.LastName} {src.Item1.FirstName}",
                    Email = src.Item1.Email,
                    PhoneNumber = src.Item1.PhoneNumber,
                    Role = src.Item1.Role,

                    OrdersCount = src.Item2.OrdersCount,
                    LastOrder = src.Item2.LastOrder
                });

            CreateMap<(User, IEnumerable<OrderDetailsSnippet>, IEnumerable<FeedbackDetailsSnippet>/*, IEnumerable<SubscriptionDetailsSnippet>*/), UserDetailsDto>()
                .ConstructUsing(src => new UserDetailsDto
                {
                    UserId = src.Item1.UserId,
                    LastName = src.Item1.LastName,
                    FirstName = src.Item1.FirstName,
                    Email = src.Item1.Email,
                    PhoneNumber = src.Item1.PhoneNumber,
                    DateOfBirth = src.Item1.DateOfBirth,
                    Role = src.Item1.Role,

                    Orders = src.Item2,
                    
                    FeedbacksCount = src.Item3.Count(),
                    Feedbacks = src.Item3,

                    // Subscriptions = src.Item4
                });

            CreateMap<Dto, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));
        }
    }
}
