using Library.DTOs.User;

<<<<<<<< HEAD:UserAPI/UserFilter.cs
namespace UserAPI
========
namespace UserAPI.Models.Filters
>>>>>>>> df2510ef3c2820bbf596e5396461943c6258e93a:UserAPI/Models/Filters/UserFilter.cs
{
    public class UserFilter
    {
        public DateTime? DateOfBirthStart { get; set; }
        public DateTime? DateOfBirthEnd { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public RoleType? Role { get; set; }
        public bool HasSubscription { get; set; }
    }
}
