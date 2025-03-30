using Library.Sorts;

namespace UserAPI.Models.Sorts
{
    public class UserSort : SortBase<User>
    {
        public Bool Alphabetical { get; set; }
        public Bool Youngest { get; set; }
        public Bool Role { get; set; }

        public override IQueryable<User> Apply(IQueryable<User> users)
        {
            users = ApplySorting(users, Alphabetical, u => u.LastName + u.FirstName);
            users = ApplySorting(users, Youngest, u => u.DateOfBirth);
            users = ApplySorting(users, Role, u => u.Role);

            return users;
        }
    }
}
