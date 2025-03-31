namespace Library.DTOs.UserRelated.Subscription;

public class SubscriptionCardDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string ImageUrl { get; set; }

    public SubscriptionCardDto()
    {
        Id = Guid.NewGuid();
        Title = string.Empty;
        ImageUrl = string.Empty;
    }
}