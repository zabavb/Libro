namespace Library.DTOs.UserRelated.User
{
    public class UserDetails
    {
        public Guid UserId { get; set; }
        public string? LastName { get; set; }
        public string FirstName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public RoleType Role { get; set; }

        public ICollection<OrderWithBookForUserDetails> Orders { get; set; }
        public int? FeedbacksCount { get; set; }
        public ICollection<FeedbackForUserDetails>? Feedbacks { get; set; }
        public ICollection<SubscriptionForUserDetails>? Subscriptions { get; set; }

        public UserDetails()
        {
            LastName = string.Empty;
            FirstName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            DateOfBirth = DateTime.Today;
        }
    }

    public class OrderWithBookForUserDetails
    {
        public string OrderUiId { get; set; } // UiId - means the shortened ID for view (after last "-")
        public ICollection<string> BookNames { get; set; }
        public float Price { get; set; }

        public OrderWithBookForUserDetails()
        {
            OrderUiId = string.Empty;
            BookNames = [];
        }
    }

    public class OrderForUserDetails
    {
        public string OrderUiId { get; set; } // UiId - means the shortened ID for view (after last "-")
        public ICollection<Guid> BookIds { get; set; }
        public float Price { get; set; }

        public OrderForUserDetails() => OrderUiId = string.Empty;
    }

    public class FeedbackForUserDetails
    {
        public string HeadLabel { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime Date { get; set; }

        public FeedbackForUserDetails()
        {
            HeadLabel = string.Empty;
            Comment = string.Empty;
        }
    }

    public class SubscriptionForUserDetails
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }

        public SubscriptionForUserDetails()
        {
            Title = string.Empty;
            ImageUrl = string.Empty;
        }
    }
}