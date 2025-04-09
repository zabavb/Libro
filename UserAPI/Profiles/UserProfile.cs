using AutoMapper;
using Library.DTOs.UserRelated.User;
using Library.Interfaces;
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

            CreateMap<(User, CollectionSnippet<OrderDetailsSnippet>, CollectionSnippet<FeedbackDetailsSnippet>),
                    UserDetailsDto>()
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
                    FeedbacksCount = src.Item3.Items.Count,
                    Feedbacks = src.Item3,

                    Subscriptions = new CollectionSnippet<SubscriptionDetailsSnippet>(
                        false,
                        src.Item1.UserSubscriptions!
                            .Select(us => us.Subscription)
                            .Select(s => new SubscriptionDetailsSnippet
                            {
                                Title = s.Title,
                                Description = s.Description,
                                ImageUrl = s.ImageUrl
                            })
                            .ToList()
                    )
                });

            CreateMap<User, UserDetailsDto>()
                .ConstructUsing(user => new UserDetailsDto
                {
                    UserId = user.UserId,
                    LastName = user.LastName,
                    FirstName = user.FirstName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    Role = user.Role,
                    Subscriptions = new CollectionSnippet<SubscriptionDetailsSnippet>(
                        false,
                        user.UserSubscriptions!
                            .Select(us => us.Subscription)
                            .Select(s => new SubscriptionDetailsSnippet
                            {
                                Title = s.Title,
                                Description = s.Description,
                                ImageUrl = s.ImageUrl
                            })
                            .ToList()
                    )
                });

            CreateMap<Dto, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));
        }
    }
}