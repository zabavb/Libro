using Library.DTOs.UserRelated.User;
using Library.Interfaces;

namespace UserAPI.Models.Filters
{
    public class UserFilter : IFilter<User>
    {
        public EmailDomen? Email { get; set; }
        public RoleType? Role { get; set; }
        public Guid? SubscriptionId { get; set; }

        public IQueryable<User> Apply(IQueryable<User> users)
        {
            if (Email.HasValue)
                users = users.Where(u => u.Email!.Equals(Email));

            if (Role.HasValue)
                users = users.Where(u => u.Role.Equals(Role));

            if (SubscriptionId.HasValue)
                users = users.Where(u => u.SubscriptionIds != null &&
                                         u.SubscriptionIds.Contains(SubscriptionId.Value));

            return users;
        }
    }
}