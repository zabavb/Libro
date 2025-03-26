using Library.DTOs.User;
using Library.Interfaces;

namespace UserAPI.Models.Filters
{
    public class UserFilter : IFilter<User>
    {
        public EmailDomen? Email { get; set; }
        public RoleType? Role { get; set; }
        // public string Subscription { get; set; }

        public IQueryable<User> Apply(IQueryable<User> users)
        {
            if (Email.HasValue)
                users = users.Where(u => u.Email!.Equals(Email));
                
            if (Role.HasValue)   
                users = users.Where(u => u.Role.Equals(Role));
            
            /* if (!string.IsNullOrEmpty(Subscription))
                users = users.Where(u => u.Equals(Subscription)); */

            return users;
        }
    }
}
