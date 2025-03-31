using Library.DTOs.UserRelated.User;
using Microsoft.EntityFrameworkCore;
using UserAPI.Models;
using UserAPI.Repositories;


namespace UserAPI.Data
{
    public class DataSeeder
    {

        public static void Seed(ModelBuilder modelBuilder)
        {

            var userData = new[]
                {
                    ("Alice", "Smith", new DateTime(1992, 5, 14), "123-456-0001", "A!ice123"),
                    ("Bob", "Johnson", new DateTime(1985, 8, 23), "123-456-0002", "B@b1985!"),
                    ("Charlie", "Brown", new DateTime(1998, 2, 17), "123-456-0003", "Ch@rlie98#"),
                    ("David", "Wilson", new DateTime(1991, 11, 30), "123-456-0004", "D@vid_91!"),
                    ("Emma", "Taylor", new DateTime(1993, 6, 25), "123-456-0005", "E!mma1993@"),
                    ("Frank", "Anderson", new DateTime(1989, 9, 10), "123-456-0006", "Fr@nk#89!"),
                    ("Grace", "Thomas", new DateTime(1995, 4, 5), "123-456-0007", "Gr@ce_95!"),
                    ("Henry", "Moore", new DateTime(1990, 12, 1), "123-456-0008", "H3nry!990"),
                    ("Isabella", "Martin", new DateTime(1987, 7, 19), "123-456-0009", "Is@bella_87!"),
                    ("Jack", "White", new DateTime(1996, 3, 12), "123-456-0010", "J@ck_1996!"),
                    ("Katie", "Harris", new DateTime(1994, 10, 22), "123-456-0011", "K@tie!94#"),
                    ("Liam", "Clark", new DateTime(1992, 1, 8), "123-456-0012", "L!amC@rk92"),
                    ("Mia", "Rodriguez", new DateTime(1988, 5, 30), "123-456-0013", "M!a1988#"),
                    ("Nathan", "Lewis", new DateTime(1997, 9, 14), "123-456-0014", "N@than97!"),
                    ("Olivia", "Walker", new DateTime(1990, 2, 26), "123-456-0015", "O!iviaW@l90"),
                    ("Paul", "Hall", new DateTime(1993, 8, 5), "123-456-0016", "P@ulH@ll93!"),
                    ("Quinn", "Allen", new DateTime(1986, 11, 11), "123-456-0017", "Qu!nnA@ll86#"),
                    ("Ryan", "Young", new DateTime(1995, 6, 18), "123-456-0018", "Ry@nY@ng95!"),
                    ("Sophia", "King", new DateTime(1991, 4, 9), "123-456-0019", "S!ophiaK!ng91"),
                    ("Tyler", "Scott", new DateTime(1999, 7, 22), "123-456-0020", "Ty!erS@tt99#"),
                    ("Uma", "Foster", new DateTime(1993, 3, 15), "123-456-0021", "U!maF@ster93"),
                    ("Victor", "Evans", new DateTime(1988, 10, 5), "123-456-0022", "V!ctorE@v88"),
                    ("Wendy", "Bennett", new DateTime(1995, 6, 20), "123-456-0023", "W@ndyB!n95"),
                    ("Xander", "Cruz", new DateTime(1992, 7, 13), "123-456-0024", "X@nderC!uz92"),
                    ("Yasmine", "Gomez", new DateTime(1990, 11, 2), "123-456-0025", "Y@sm!neG90"),
                    ("Zachary", "Reed", new DateTime(1986, 4, 29), "123-456-0026", "Z@chR!ed86"),
                    ("Amber", "Perry", new DateTime(1997, 9, 8), "123-456-0027", "A!mberP@r97"),
                    ("Brandon", "Morris", new DateTime(1994, 12, 19), "123-456-0028", "B@randonM94"),
                    ("Catherine", "Stewart", new DateTime(1989, 5, 25), "123-456-0029", "C@thSt!w89"),
                    ("Daniel", "Bell", new DateTime(1996, 8, 30), "123-456-0030", "D@nielB!ll96"),
                    ("Eleanor", "Howard", new DateTime(1991, 2, 16), "123-456-0031", "E!eanorH@w91"),
                    ("Felix", "Ward", new DateTime(1987, 6, 11), "123-456-0032", "F@lixW@rd87"),
                    ("Gabriella", "Bryant", new DateTime(1993, 10, 27), "123-456-0033", "G@briBry93"),
                    ("Harrison", "Wood", new DateTime(1998, 1, 7), "123-456-0034", "H@rrisonW98"),
                    ("Ivy", "Hayes", new DateTime(1990, 3, 14), "123-456-0035", "IvyH@yes90"),
                    ("Jason", "Russell", new DateTime(1985, 12, 22), "123-456-0036", "J@sonR!ss85")
                };

            #region Manual Seed

            var user1 = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                Role = RoleType.ADMIN
            };

            var sub1 = new Subscription
            {
                SubscriptionId = Guid.NewGuid(),
                Title = "Premium Plan",
                EndDate = DateTime.Now.AddYears(1),
                UserId = user1.UserId
            };

            user1.SubscriptionId = sub1.SubscriptionId;

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
                Role = RoleType.MODERATOR
            };

            var sub2 = new Subscription
            {
                SubscriptionId = Guid.NewGuid(),
                Title = "Premium Plan",
                EndDate = DateTime.Now.AddYears(2),

                UserId = user2.UserId
            };

            user2.SubscriptionId = sub2.SubscriptionId;
            
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

            #endregion

            var users = new List<User>();
            var subscriptions = new List<Subscription>();
            var passwords = new List<Password>();

            foreach (var (firstName, lastName, dob, phone, passwordString) in userData)
            {
                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dob,
                    Email = $"{firstName.ToLower()}.{lastName.ToLower()}@example.com",
                    PhoneNumber = phone,
                    Role = RoleType.USER,
                };
                var salt = PasswordRepository.GenerateSalt();
                var hash = PasswordRepository.HashPassword(passwordString, salt);
                var pass = new Password
                {
                    PasswordId = Guid.NewGuid(),
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    UserId = user.UserId
                };
                var sub = new Subscription
                {
                    SubscriptionId = Guid.NewGuid(),
                    Title = "Basic Plan",
                    EndDate = DateTime.Now.AddMonths(3),
                    UserId = user.UserId
                };
                user.PasswordId = pass.PasswordId;
                user.SubscriptionId = sub.SubscriptionId;


                users.Add(user);
                subscriptions.Add(sub);
                passwords.Add(pass);
            }

            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Subscription>().HasData(subscriptions);
            modelBuilder.Entity<Password>().HasData(passwords);
        }
    }
}
