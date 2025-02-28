using Microsoft.EntityFrameworkCore;
using UserAPI.Models;
using UserAPI.Repositories;


namespace UserAPI.Data
{
    public class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var passwordRepo = new PasswordRepository();


            var user1 = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                Role = Library.DTOs.User.RoleType.GUEST,
                Subscription = null,
                Password = null!,

            };

            var sub1 = new Subscription
            {
                SubscriptionId = Guid.NewGuid(),
                Title = "Premium Plan",
                EndDate = DateTime.Now.AddYears(1),
                User = user1,
                UserId = user1.UserId
            };
            user1.Subscription = sub1;


            var salt1 = passwordRepo.GenerateSalt();
            var hash1 = passwordRepo.HashPassword("12", salt1);

            var pass1 = new Password
            {
                PasswordId = Guid.NewGuid(),
                PasswordHash = hash1,
                PasswordSalt = salt1,
                UserId = user1.UserId,
                User = user1,
                
            };

            user1.Password = pass1;
            user1.PasswordId = pass1.PasswordId;

            var user2 = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 5, 15),
                Email = "jane.smith@example.com",
                PhoneNumber = "987-654-3210",
                Role = Library.DTOs.User.RoleType.GUEST,
                Subscription = null,
                Password = null!,
            };

            var sub2 = new Subscription
            {
                SubscriptionId = Guid.NewGuid(),
                Title = "Premium Plan",
                EndDate = DateTime.Now.AddYears(2),
                User = user2,
                UserId = user2.UserId
            };
            user2.Subscription = sub2;
            
            var salt2 = passwordRepo.GenerateSalt();
            var hash2 = passwordRepo.HashPassword("123", salt2);

            var pass2 = new Password
            {
                PasswordId = Guid.NewGuid(),
                PasswordHash = hash2,
                PasswordSalt = salt2,
                UserId = user1.UserId,
                User = user1,
            };
            user2.Password = pass2;
            user2.PasswordId = pass2.PasswordId;

            modelBuilder.Entity<User>().HasData(user1, user2);
            modelBuilder.Entity<Subscription>().HasData(sub1, sub2);
            modelBuilder.Entity<Password>().HasData(pass1, pass2);
        }
    }
    
}
