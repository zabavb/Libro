using Library.DTOs.UserRelated.User;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using UserAPI.Models;
using UserAPI.Models.Filters;
using UserAPI.Models.Subscription;
using UserAPI.Services.Interfaces;

namespace UserAPI.Data
{
    public class DataSeeder(
        UserDbContext context,
        IAvatarService avatarService,
        IS3StorageService storageService,
        ILogger<DataSeeder> logger,
        IConfiguration configuration)
    {
        private readonly UserDbContext _context = context;
        private readonly IAvatarService _avatarService = avatarService;
        private readonly IS3StorageService _storageService = storageService;
        private readonly ILogger<DataSeeder> _logger = logger;
        private readonly string _seedPassword = configuration["SeedPassword"]!;

        private static readonly Random Random = new();
        private const string UserImagesSeedDir = "SeedImages/Users";
        private const string SubscriptionImagesSeedDir = @"Data\SeedImages\Subscriptions";

        private static readonly string[] ModeratorFirstNames =
        [
            "Alice", "Bob", "Charlie", "Diana",
            "Edward", "Fiona", "George", "Hannah",
            "Ian", "Julia", "Kevin", "Laura",
            "Martin", "Nina", "Oscar", "Paula",
            "Quentin", "Rachel", "Steve"
        ];

        private static readonly string[] ModeratorLastNames =
        [
            "Anderson", "Brown", "Clark", "Davis",
            "Evans", "Foster", "Garcia", "Hughes",
            "Irwin", "Johnson", "King", "Lewis",
            "Mitchell", "Nelson", "Owens", "Parker",
            "Quinn", "Roberts", "Scott"
        ];

        private static readonly string[] FirstNames =
        {
            "Aaron", "Bella", "Cameron", "Dana", "Ethan",
            "Faith", "Gavin", "Hailey", "Isaac", "Jasmine",
            "Kyle", "Lily", "Miles", "Naomi", "Oliver",
            "Penelope", "Quinn", "Riley", "Sean", "Tara",
            "Umar", "Vera", "Wyatt", "Xena", "Yara",
            "Zane", "Ava", "Brandon", "Caitlyn", "Derek",
            "Eliza", "Finn", "Grace", "Harper", "Ian",
            "Jack", "Kara", "Leo", "Maya", "Noah",
            "Opal", "Peyton", "Quincy", "Rose", "Sam",
            "Tessa", "Uri", "Violet", "Will", "Xavier",
            "Yasmine", "Zack", "Amber", "Brett", "Cassie",
            "Dylan", "Ella", "Freddie", "Gwen", "Hank",
            "Isla", "Jake", "Kate", "Landon", "Mila",
            "Nico", "Owen", "Piper", "Quiana", "Ray",
            "Sara", "Tom", "Una", "Val", "Wren",
            "Ximena", "Yosef", "Zelda", "Ash", "Blaire"
        };

        private static readonly string[] LastNames =
        {
            "Adams", "Baker", "Carter", "Dixon", "Ellis",
            "Ferguson", "Grant", "Harris", "Ingram", "Jacobs",
            "Keller", "Larson", "Morris", "Nash", "O'Brien",
            "Price", "Qualls", "Reed", "Smith", "Taylor",
            "Underwood", "Vance", "Walker", "Xiong", "Young",
            "Zimmerman", "Abbott", "Barnes", "Chavez", "Dalton",
            "Edwards", "Fitzgerald", "Gibson", "Howard", "Iverson",
            "James", "Knight", "Lane", "Moore", "Nelson",
            "Ortega", "Porter", "Quintana", "Ramirez", "Stewart",
            "Thomas", "Upton", "Vargas", "White", "Xu",
            "York", "Zamora", "Austin", "Blake", "Coleman",
            "Drake", "Erickson", "Ford", "Green", "Hayes",
            "Irving", "Jefferson", "Kim", "Long", "Martin",
            "Nguyen", "Owens", "Powell", "Robinson", "Shaw",
            "Travis", "Ulrich", "Vega", "Watson", "Xander",
            "Yates", "Zhou", "Archer", "Bryant", "Caldwell"
        };

        public async Task SeedAsync()
        {
            if (await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Seeding skipped. Data already exists.");
                return;
            }

            var users = await SeedUsersAsync();
            var passwords = SeedPasswords(users);
            var subscriptions = await SeedSubscriptionsAsync();
            var userSubscriptions = SubscribeUsers(users, subscriptions);

            await InsertSeeds(users, passwords, subscriptions, userSubscriptions);
        }

        private async Task<List<User>> SeedUsersAsync()
        {
            var users = new List<User>
            {
                new("admin", "admin", RdmDateOfBirth(), RdmEmailDomen(), RdmPhoneNumber(), RoleType.ADMIN)
            };

            for (var i = 0; i < ModeratorFirstNames.Length; i++)
                users.Add(new User(ModeratorFirstNames[i], ModeratorLastNames[i], RdmDateOfBirth(), RdmEmailDomen(),
                    RdmPhoneNumber(), RoleType.MODERATOR));

            for (var i = 0; i < FirstNames.Length; i++)
                users.Add(new User(FirstNames[i], LastNames[i], RdmDateOfBirth(), RdmEmailDomen(), RdmPhoneNumber()));

            foreach (var user in users)
            {
                var image = await _avatarService.GenerateAvatarAsync(user.FirstName, user.LastName);
                if (image == null)
                    _logger.LogError("Failed to generate avatar for user {UserFirstName} {UserLastName}",
                        user.FirstName, user.LastName);
                else
                {
                    var imageUrl = await _storageService.UploadAsync(
                        GlobalDefaults.BucketName,
                        GlobalDefaults.UserImagesFolder,
                        user.UserId,
                        image,
                        GlobalDefaults.UserImagesSize);

                    user.ImageUrl = imageUrl;
                }
            }

            _logger.LogInformation("Created {UsersCount} users with their images.", users.Count);
            return users;
        }

        private List<Password> SeedPasswords(List<User> users)
        {
            var passwords = new List<Password>();
            var salt = PasswordExtensions.GenerateSalt()!;
            var hash = PasswordExtensions.HashPassword(_seedPassword, salt);

            users.ForEach(user => passwords.Add(new Password(hash, salt, user.UserId)));

            _logger.LogInformation("Created {PasswordsCount} passwords.", passwords.Count);
            return passwords;
        }

        private async Task<List<Subscription>> SeedSubscriptionsAsync()
        {
            var description = "Subscription for free delivery of orders from Yakaboo throughout Ukraine." +
                              "Valid for all orders over 100 UAH for 1 year from the moment of registration." +
                              "There are no restrictions on the number of orders.";
            var subscriptions = new List<Subscription>
            {
                new("Subscription 365", 365, 365, "Free shipping for a year for 365₴", description)
            };

            for (var i = 0; i < subscriptions.Count; i++)
            {
                var imageUrl = await _storageService.UploadAsync(
                    GlobalDefaults.BucketName,
                    GlobalDefaults.SubscriptionImagesFolder,
                    subscriptions[i].SubscriptionId,
                    GetImageAsFormFile($"image_{i}.png", SubscriptionImagesSeedDir),
                    GlobalDefaults.SubscriptionImagesSize);

                subscriptions[i].ImageUrl = imageUrl;
            }

            _logger.LogInformation(
                "Created {SubscriptionsCount} subscriptions with their images.", subscriptions.Count);
            return subscriptions;
        }

        private List<UserSubscription> SubscribeUsers(List<User> users, List<Subscription> subscriptions)
        {
            var userSubscriptions = new List<UserSubscription>();
            var usedPairs = new HashSet<(Guid UserId, Guid SubscriptionId)>();
            var desiredCount = Random.Next(users.Count);

            while (userSubscriptions.Count < desiredCount)
            {
                var user = users[Random.Next(users.Count)];
                var subscription = subscriptions[Random.Next(subscriptions.Count)];
                var pair = (user.UserId, subscription.SubscriptionId);

                if (!usedPairs.Add(pair))
                    continue;

                userSubscriptions.Add(new UserSubscription
                {
                    UserId = user.UserId,
                    SubscriptionId = subscription.SubscriptionId,
                    ExpirationDate = DateTime.UtcNow.AddDays(subscription.ExpirationDays)
                });
            }

            _logger.LogInformation("Subscribed {Count} unique users to subscriptions.", userSubscriptions.Count);
            return userSubscriptions;
        }

        private async Task InsertSeeds(
            List<User> users,
            List<Password> passwords,
            List<Subscription> subscriptions,
            List<UserSubscription> userSubscriptions)
        {
            await _context.Users.AddRangeAsync(users);
            _logger.LogInformation("Inserted {UsersCount} users into DB.", users.Count);

            await _context.Passwords.AddRangeAsync(passwords);
            _logger.LogInformation("Inserted {PasswordsCount} passwords into DB.", passwords.Count);

            await _context.Subscriptions.AddRangeAsync(subscriptions);
            _logger.LogInformation("Inserted {SubscriptionsCount} subscriptions into DB.", subscriptions.Count);

            await _context.UserSubscriptions.AddRangeAsync(userSubscriptions);
            _logger.LogInformation("Inserted {Count} UserSubscription entities into DB.", userSubscriptions.Count);

            await _context.SaveChangesAsync();
        }

        private static DateTime RdmDateOfBirth()
        {
            var start = new DateTime(1970, 1, 1);
            var end = new DateTime(2017, 12, 31);
            int range = (end - start).Days;
            return start.AddDays(Random.Next(range));
        }

        private static string RdmEmailDomen()
        {
            var values = Enum.GetValues<EmailDomen>();
            var selected = values[Random.Next(values.Length)];
            return selected.ToString().ToLower();
        }

        private static string RdmPhoneNumber()
        {
            var part1 = Random.Next(100, 1000);
            var part2 = Random.Next(100, 1000);
            var part3 = Random.Next(1000, 10000);
            return $"{part1}-{part2}-{part3}";
        }

        private FormFile GetImageAsFormFile(string fileName, string seedDir)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), seedDir, fileName);
            if (!File.Exists(filePath))
                _logger.LogError("Image not found: {FilePath}", filePath);

            var stream = File.OpenRead(filePath);

            return new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = GetContentType(fileName)
            };
        }

        private static string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}