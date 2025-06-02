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

        private static readonly List<Guid> userIds = new List<Guid>
        {
            new("eb65e5c5-a3bd-4da7-9c43-e722c49c0151"),
            new("0497d9e4-ec6b-4277-a268-18f61104e140"),
            new("56a1c91f-8b00-43fa-adac-19b67559c48d"),
            new("47f7e164-97d2-4f0c-ae58-173b38554658"),
            new("da4f5a5b-f2c3-4608-bd34-bf0b6431e75c"),
            new("1d848cea-362d-40d9-8821-4bc984e5e25a"),
            new("d605c572-6210-4840-918c-5348fe63815a"),
            new("34c5f3c9-9b9a-490c-91c2-1a88a22226bc"),
            new("f3c715ba-7478-43de-a35a-08cef0d55c27"),
            new("b86c042d-a6f0-4cb8-90b4-31060f0af325"),
            new("85ddedba-fb4a-4ba9-aa1c-ad96585269a5"),
            new("472855a2-96a7-46b2-9492-9b25af4fab98"),
            new("6d79fbcb-18f5-4e4d-a942-617a5bb8d930"),
            new("9bcda4f1-4a3c-48d3-8afc-f9baa08ca08b"),
            new("0d8ca9dd-3089-48b0-afa3-e045cd9f05c7"),
            new("6df10759-2eed-4d49-ada2-731f852bdb27"),
            new("8e8916a0-803e-4dd8-a3db-9475d966662b"),
            new("c901583d-4b89-427b-80dd-83a6730abe82"),
            new("5caf92ea-a753-467b-bb75-098c4c3bd708"),
            new("a383537b-ca95-44ee-b3f6-d37c558f3887"),
            new("a5c42ed4-d66c-4cb8-93f7-b8e1c7130461"),
            new("54ca91d2-1f1c-404c-88dc-0771d17b789c"),
            new("2b38343d-7297-4b61-9a5c-915786e34042"),
            new("92db1eac-dc83-4d61-bc47-f7411cae7a5b"),
            new("7d5a3c6b-b239-4694-ab38-c6e00cbc3b02"),
            new("5fb69107-d2f9-46ef-9d23-090c1f935189"),
            new("86b95800-c873-449b-b247-899df293e428"),
            new("03c6c24d-b23a-48f4-bd74-190c8db24b4d"),
            new("68b209fd-24b6-4fbd-9be9-7f48c7c820ac"),
            new("757ad1cb-8a60-4913-83bf-68e0a63ceeee"),
            new("49888348-02b4-4156-ad80-17ccf3faab14"),
            new("5713cecb-5592-4f20-99d7-662c7c09c0c2"),
            new("1b16ca5d-d7e0-488a-aaed-b57433fa8507"),
            new("f37f7c84-a6ad-4baf-b120-309400b1fa99"),
            new("b9426929-5b20-4255-bdd0-7cfa96476f69"),
            new("d2291bb2-3344-4239-999b-f0916f4bd355"),
            new("4313b260-5b2b-4c82-b104-5528484b36bf"),
            new("264891fc-4282-4389-b966-8b038a20e77c"),
            new("2e956173-f632-49ba-800e-609b5931c307"),
            new("c118c4bb-c54c-4f7d-a47c-a55ff006ae69"),
            new("68b0e408-13ea-4d50-9eb3-cabb65dfa809"),
            new("985ba966-69a3-4656-9b3c-07674cb507fd"),
            new("381f5c3d-f97b-4165-8e4c-5b557d8be7f1"),
            new("e2b2e8a0-9ff8-4d9d-9115-c76cc5f23426"),
            new("a88d6cf9-6303-4aaa-9a88-2baaae5cf7c8"),
            new("93c016de-bb2b-48bc-939c-a4e0967e33d8"),
            new("3ce4fdb7-b5ce-45d8-9b2b-1ae83826bbc6"),
            new("a9117fb3-2309-4b46-b18f-1b318409afad"),
            new("a4b2c702-da83-43c9-b02c-b8c8b7f064bd"),
            new("01acf3fd-3340-4e90-810d-3fb0a29c4afc"),
            new("0d22f83b-d82a-434a-a823-8e28091c3e8f"),
            new("533ed940-7858-44cc-a760-239c113e5186"),
            new("8a000acb-4f2c-4c7e-93b1-fa7cd7ab2005"),
            new("96704011-45cd-4c78-abc9-1a679c2a9178"),
            new("526af88d-c75a-4387-8349-f74ac0bc6534"),
            new("63b20b1d-14a4-4602-a2b4-bc146dbeea8b"),
            new("bb285cfb-3c43-4365-b72a-5308b8af4766"),
            new("5b2727a7-6162-4900-8681-ad8a39d76aa3"),
            new("cfd22068-6ceb-4355-a3a4-335de99de5ee"),
            new("a4ba8212-cd62-4ad7-a885-72018d25fa99"),
            new("79dc4da1-3474-44f9-ae19-46f78460996e"),
            new("b48caf74-047f-4fda-932c-fbf9a965515e"),
            new("1b7cfd58-e29a-462f-85df-a97a91f0907f"),
            new("e8899dda-67fd-426f-a883-f7ac9b8397d5"),
            new("e90bb814-5aa0-4f06-9a93-a9baabc2e730"),
            new("90ca2797-44ae-47d9-a93f-d0428aa0a103"),
            new("47a6506d-1124-4eff-9943-1686214ce41c"),
            new("7f29762c-ebe9-4896-aea1-7d25ea34a706"),
            new("1b40c1b4-4858-4f25-bc03-3cddefdd1187"),
            new("b1912300-56d2-41d1-8b66-b5cc2f84429f"),
            new("ac79062f-9c4d-4943-baef-9f3c9df6c85c"),
            new("b271626a-0fef-4451-9f7b-5cbbac62ac21"),
            new("0d5254b1-aad8-45ed-b718-4d72741c7b24"),
            new("3cedb8d2-679e-40e4-afe1-b012758f2ba5"),
            new("13369848-d694-4f16-aa02-a12f882fdcba"),
            new("dbe0656a-af34-4b6a-bdee-3bea66b0e9c7"),
            new("946e6768-9962-465b-8a6c-ad3d97d377ea"),
            new("d55c56ff-87e6-4141-a34a-ab71190b4223"),
            new("f13beee9-561c-4086-9bc9-b21faf7f4c6c"),
            new("d0e827e7-6339-43fc-8959-4713a4ea535d")
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
                users.Add(new User(userIds[i] ,FirstNames[i], LastNames[i], RdmDateOfBirth(), RdmEmailDomen(), RdmPhoneNumber()));

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

            await _context.Users.AddRangeAsync(users);
            _logger.LogInformation("Inserted {UsersCount} users into DB.", users.Count);
            await _context.SaveChangesAsync();

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

            await _context.Subscriptions.AddRangeAsync(subscriptions);
            _logger.LogInformation("Inserted {SubscriptionsCount} subscriptions into DB.", subscriptions.Count);
            await _context.SaveChangesAsync();

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
/*            await _context.Users.AddRangeAsync(users);
            _logger.LogInformation("Inserted {UsersCount} users into DB.", users.Count);
            await _context.SaveChangesAsync();*/

            await _context.Passwords.AddRangeAsync(passwords);
            _logger.LogInformation("Inserted {PasswordsCount} passwords into DB.", passwords.Count);

/*            await _context.Subscriptions.AddRangeAsync(subscriptions);
            _logger.LogInformation("Inserted {SubscriptionsCount} subscriptions into DB.", subscriptions.Count);
            await _context.SaveChangesAsync();*/

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