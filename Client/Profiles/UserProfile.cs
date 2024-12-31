using AutoMapper;
using Client.Models.User;
using Library.DTOs.User;

namespace Client.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, ManageUserViewModel>();
        }
    }
}
