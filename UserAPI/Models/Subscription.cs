namespace UserAPI.Models
{
    public class Subscription
    {
        public Guid SubscriptionId { get; set; }
        public string Title { get; set; }
        public int ExpirationDays { get; set; }
        public float Price { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Subscription()
        {
            SubscriptionId = Guid.NewGuid();
            Title = string.Empty;
            ImageUrl = string.Empty;
            User = null!;
        }
    }
}