using AutoMapper;
using DeadAspClient.Models.UserEntities.User;
using Library.DTOs.User;

namespace DeadAspClient.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, ManageUserViewModel>();
        }
    }
}
