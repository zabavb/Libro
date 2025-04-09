using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace UserAPI.Services
{
    public class AvatarService
    {
        private const int ImageSize = 80;
        private const int FontSize = 35;

        public async Task<byte[]> GenerateAvatarAsync(string firstName, string? lastName)
        {
            string initials = GetInitials(firstName, lastName);
            var randomColor = GetRandomColor();
            
            using var image = new Image<Rgba32>(ImageSize, ImageSize, randomColor);
            var font = SystemFonts.CreateFont("Arial", FontSize, FontStyle.Bold);
            
            var textOptions = new TextOptions(font)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Origin = new PointF(ImageSize / 2, ImageSize / 2)
            };
            
            image.Mutate(ctx => ctx.DrawText((RichTextOptions)textOptions, initials, Color.White));
            
            using var ms = new MemoryStream();
            await image.SaveAsPngAsync(ms);
            return ms.ToArray();
        }

        private static string GetInitials(string firstName, string? lastName)
        {
            char firstInitial = !string.IsNullOrWhiteSpace(firstName) ? char.ToUpper(firstName[0]) : ' ';
            char lastInitial = !string.IsNullOrWhiteSpace(lastName) ? char.ToUpper(lastName[0]) : ' ';
            return $"{lastInitial}{firstInitial}";
        }

        private static Rgba32 GetRandomColor()
        {
            Random rnd = new();
            return new Rgba32(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        }
    }
}
