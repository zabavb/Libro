using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class AvatarService : IAvatarService
    {
        private const int ImageSize = 64;
        private const int FontSize = 35;

        public async Task<IFormFile?> GenerateAvatarAsync(string firstName, string? lastName)
        {
            var initials = GetInitials(firstName, lastName);
            try
            {
                var randomColor = GetRandomColor();

                using var image = new Image<Rgba32>(ImageSize, ImageSize, randomColor);
                var font = SystemFonts.CreateFont("Arial", FontSize, FontStyle.Bold);

                var richTextOptions = new RichTextOptions(font)
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Origin = new PointF(ImageSize / 2, ImageSize / 2)
                };

                image.Mutate(ctx => ctx.DrawText(richTextOptions, initials, Color.White));

                using var ms = new MemoryStream();
                await image.SaveAsPngAsync(ms);
                var msImage = ms.ToArray();

                var stream = new MemoryStream(msImage);
                IFormFile formFile = new FormFile(stream, 0, msImage.Length, "file", "avatar.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                return formFile;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Database error for user's avatar generation.", ex);
            }
        }

        private static string GetInitials(string firstName, string? lastName)
        {
            var firstInitial = !string.IsNullOrWhiteSpace(firstName) ? char.ToUpper(firstName[0]) : ' ';
            var lastInitial = !string.IsNullOrWhiteSpace(lastName) ? char.ToUpper(lastName[0]) : ' ';
            return $"{lastInitial}{firstInitial}";
        }

        private static Rgba32 GetRandomColor()
        {
            Random rnd = new();
            var r = rnd.Next(200);
            var g = rnd.Next(200);
            var b = rnd.Next(200);
            return new Rgba32((byte)r, (byte)g, (byte)b);
        }
    }
}