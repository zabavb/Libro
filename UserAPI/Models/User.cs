﻿using Library.DTOs.User;

namespace UserAPI.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public RoleType Role { get; set; }
        public string ImageUrl { get; set; }
        public Guid PasswordId { get; set; }
        public Password Password { get; set; }
        public Guid? SubscriptionId { get; set; }
        public Subscription? Subscription { get; set; }

        public User()
        {
            UserId = Guid.NewGuid();
            FirstName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateTime.Today;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            ImageUrl = string.Empty;
            Role = RoleType.GUEST;
            Password = null!;
        }
    }
}