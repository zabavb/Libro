using SixLabors.ImageSharp;

namespace UserAPI
{
    public static class GlobalDefaults
    {
        public const int pageNumber = 1;
        public const int pageSize = 10;

        public const int cacheExpirationTime = 10;

        public static readonly string BucketName;

        public const string UserImagesFolder = "user/images/";
        public static readonly Size UserImagesSize = new(64, 64);

        public const string SubscriptionImagesFolder = "subscription/images/";
        public static readonly Size SubscriptionImagesSize = new(190, 190);

        static GlobalDefaults()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            BucketName = config["AWS:BucketName"] ??
                         throw new Exception("BucketName is not set in appsettings.json");
        }
    }
}