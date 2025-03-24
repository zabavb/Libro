using Library.DTOs.User;

namespace Library.DTOs.UserRelated.User
{
    public class UserDetailsDto
    {
        public Guid UserId { get; set; }
        public string? LastName { get; set; }
        public string FirstName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public RoleType Role { get; set; }

        public IEnumerable<OrderDetailsSnippet>? Orders { get; set; }
        public int? FeedbacksCount { get; set; }
        public IEnumerable<FeedbackDetailsSnippet>? Feedbacks { get; set; }
        // public IEnumerable<SubscriptionDetailsSnippet> Subscriptions { get; set; }

        public UserDetailsDto()
        {
            LastName = string.Empty;
            FirstName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            DateOfBirth = DateTime.Today;
            Orders = [];
            Feedbacks = [];
            // Subscriptions = [];
        }
    }

    public class OrderDetailsSnippet
    {
        public string OrderUiId { get; set; }   // UiId - means the shortened Id for view (after last "-")
        public IEnumerable<string> BookNames { get; set; }
        public float Price { get; set; }

        public OrderDetailsSnippet()
        {
            OrderUiId = string.Empty;
            BookNames = [];
        }
    }

    public class FeedbackDetailsSnippet
    {
        public string HeadLabel { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }

        public FeedbackDetailsSnippet()
        {
            HeadLabel = string.Empty;
            Comment = string.Empty;
        }
    }

    /*public class SubscriptionDetailsSnippet
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public SubscriptionDetailsSnippet()
        {
            Title = string.Empty;
            Description = string.Empty;
            ImageUrl = string.Empty;
        }
    }*/
}
