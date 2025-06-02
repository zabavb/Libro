namespace UserAPI.Models.Subscription
{
    public class Subscription
    {
        public Guid SubscriptionId { get; set; }
        public string Title { get; set; }
        public int ExpirationDays { get; set; }
        public float Price { get; set; }
        public string Subdescription { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<UserSubscription>? UserSubscriptions { get; set; }

        public Subscription(Guid id, string title, int expirationDays, float price, string subdescription, string? description)
        {
            SubscriptionId = id;
            Title = title;
            ExpirationDays = expirationDays;
            Price = price;
            Subdescription = subdescription;
            Description = description;
        }

        public Subscription()
        {
            SubscriptionId = Guid.NewGuid();
            Title = string.Empty;
            Subdescription = string.Empty;
            ImageUrl = string.Empty;
        }
    }
}