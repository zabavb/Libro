namespace UserAPI.Models.Subscription
{
    public class UserSubscription
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }

        public DateTime ExpirationDate { get; set; }
        
        

        public UserSubscription()
        {
            User = null!;
            Subscription = null!;
        }
    }
}