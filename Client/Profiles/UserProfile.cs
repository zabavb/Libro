using AutoMapper;
using Client.Models.User;
using Library.UserEntities;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
