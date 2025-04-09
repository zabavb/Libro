namespace UserAPI
{
    public static class GlobalDefaults
    {
        public const int pageNumber = 1;
        public const int pageSize = 10;
        public const int cacheExpirationTime = 10;
        public static readonly string BucketName;

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