using Library.DTOs.UserRelated.User;
using Library.Interfaces;

namespace UserAPI.Models.Filters
{
    public class UserFilter : IFilter<User>
    {
        public EmailDomen? Email { get; set; }
        public RoleType? RoleFilter { get; set; }
        public Guid? SubscriptionId { get; set; }

        public IQueryable<User> Apply(IQueryable<User> users)
        {
            if (Email.HasValue)
            {
                var normalizedEmail = $"{Email.Value.ToString().ToLower()}.com";
                users = users.Where(u => u.Email!.EndsWith(normalizedEmail));
            }

            if (RoleFilter.HasValue)
                users = users.Where(u => u.Role == RoleFilter);

            if (SubscriptionId.HasValue)
                users = users.Where(u => u.SubscriptionIds != null &&
                                         u.SubscriptionIds.Contains(SubscriptionId.Value));

            return users;
        }
    }
}