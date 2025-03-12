using Microsoft.EntityFrameworkCore;
using UserAPI.Models;
using UserAPI.Repositories;


namespace UserAPI.Data
{
    public class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {


            var user1 = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                Role = Library.DTOs.User.RoleType.USER
            };

            var sub1 = new Subscription
            {
                SubscriptionId = Guid.NewGuid(),
                Title = "Premium Plan",
                EndDate = DateTime.Now.AddYears(1),
                UserId = user1.UserId
            };


            var salt1 = PasswordRepository.GenerateSalt();
            var hash1 = PasswordRepository.HashPassword("123456Aa!", salt1);

            var pass1 = new Password
            {
                PasswordId = Guid.NewGuid(),
                PasswordHash = hash1,
                PasswordSalt = salt1,
                UserId = user1.UserId
            };

            user1.PasswordId = pass1.PasswordId;

            var user2 = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1985, 5, 15),
                Email = "jane.smith@example.com",
                PhoneNumber = "987-654-3210",
                Role = Library.DTOs.User.RoleType.ADMIN
            };

            var sub2 = new Subscription
            {
                SubscriptionId = Guid.NewGuid(),
                Title = "Premium Plan",
                EndDate = DateTime.Now.AddYears(2),

                UserId = user2.UserId
            };
            
            var salt2 = PasswordRepository.GenerateSalt();
            var hash2 = PasswordRepository.HashPassword("abcdefg123@", salt2);

            var pass2 = new Password
            {
                PasswordId = Guid.NewGuid(),
                PasswordHash = hash2,
                PasswordSalt = salt2,
                UserId = user2.UserId,
            };
            user2.PasswordId = pass2.PasswordId;

            modelBuilder.Entity<User>().HasData(user1, user2);
            modelBuilder.Entity<Subscription>().HasData(sub1, sub2);
            modelBuilder.Entity<Password>().HasData(pass1, pass2);
        }
    }
    
}
