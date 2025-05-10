
using Microsoft.EntityFrameworkCore;
using BookAPI.Models;
using Library.Common;

namespace BookAPI.Data
{
    public static class DataSeeder
    {
        public static async Task Seed(ModelBuilder modelBuilder, S3StorageService storageService)
        {
            var filesHelper = new FilesHelper(storageService, "libro-book");

            var bookIds = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var localAudioPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Files", "MBp1.mp3");
            var pdflocalAudioPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Files", "Ticks.pdf");


            var imagePaths = new Dictionary<string, string>
            {
                { "Місто зі скла", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJSVPcBg9gdzf2mit382PYIbFkkDbn-JB7jA&s" },
                { "Тіні минулого", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJSVPcBg9gdzf2mit382PYIbFkkDbn-JB7jA&s" },
                { "Емоційний інтелект", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJSVPcBg9gdzf2mit382PYIbFkkDbn-JB7jA&s" }
            };
            var audios = new List<string>
            {
                "https://commondatastorage.googleapis.com/codeskulptor-assets/jump.ogg" ,
            };

            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Фантастика" },
                new Category { Id = Guid.NewGuid(), Name = "Детектив" },
                new Category { Id = Guid.NewGuid(), Name = "Наукова література" },
                new Category { Id = Guid.NewGuid(), Name = "Історія" },
                new Category { Id = Guid.NewGuid(), Name = "Психологія" },
                new Category { Id = Guid.NewGuid(), Name = "Філософія" },
                new Category { Id = Guid.NewGuid(), Name = "Економіка" },
                new Category { Id = Guid.NewGuid(), Name = "Мистецтво" },
                new Category { Id = Guid.NewGuid(), Name = "Подорожі" },
                new Category { Id = Guid.NewGuid(), Name = "Кулінарія" }
            };
            modelBuilder.Entity<Category>().HasData(categories);

            // Додавання підкатегорій
            var subCategories = new List<SubCategory>
            {
                // Підкатегорії для "Фантастика"
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Name = "Космічна фантастика" },
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Name = "Фентезі" },
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Name = "Альтернативна історія" },

                // Підкатегорії для "Детектив"
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[1].Id, Name = "Кримінальний детектив" },
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[1].Id, Name = "Трилер" },
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[1].Id, Name = "Поліцейський детектив" },

                // Підкатегорії для "Наукова література"
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[2].Id, Name = "Фізика" },
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[2].Id, Name = "Біологія" },
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[2].Id, Name = "Астрономія" },

                // Підкатегорії для "Історія"
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[3].Id, Name = "Стародавній світ" },
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[3].Id, Name = "Середньовіччя" },
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[3].Id, Name = "Нова історія" },

                // Підкатегорії для "Психологія"
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[4].Id, Name = "Емоційний інтелект" },
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[4].Id, Name = "Саморозвиток" },
                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[4].Id, Name = "Психологія стосунків" }
            };
            modelBuilder.Entity<SubCategory>().HasData(subCategories);

            var publishers = new List<Publisher>
            {
                new Publisher
                {
                    Id = Guid.NewGuid(),
                    Name = "Світло Знань",
                    Description = "Видавництво, що спеціалізується на науковій літературі."
                },
                new Publisher
                {
                    Id = Guid.NewGuid(),
                    Name = "Книжковий Дім \"Грань\"",
                    Description = "Відоме своїми захопливими детективами та психологічними трилерами."
                },
                new Publisher
                {
                    Id = Guid.NewGuid(),
                    Name = "ФантаЛайн",
                    Description = "Видає найкращі зразки фантастики та фентезі."
                },
                new Publisher
                {
                    Id = Guid.NewGuid(),
                    Name = "ІстФакт",
                    Description = "Спеціалізується на якісній історичній літературі."
                },
                new Publisher
                {
                    Id = Guid.NewGuid(),
                    Name = "Психея Прес",
                    Description = "Пропонує книжки з психології, саморозвитку та стосунків."
                }
            };

            modelBuilder.Entity<Publisher>().HasData(publishers);


            var authors = new List<Author>
            {
                new Author
                {
                    Id = Guid.NewGuid(),
                    Name = "Джон Сміт",
                    Biography = "Відомий письменник у жанрі фантастики, автор бестселерів.",
                    DateOfBirth = new DateTime(1975, 4, 23),
                    Citizenship = "США"
                },
                new Author
                {
                    Id = Guid.NewGuid(),
                    Name = "Анна Браун",
                    Biography = "Авторка детективних романів, відома своїми захоплюючими сюжетами.",
                    DateOfBirth = new DateTime(1982, 11, 5),
                    Citizenship = "Велика Британія"
                },
                new Author
                {
                    Id = Guid.NewGuid(),
                    Name = "Марія Коваль",
                    Biography = "Психологиня, авторка книг про емоційний інтелект та стрес.",
                    DateOfBirth = new DateTime(1990, 2, 14),
                    Citizenship = "Україна"
                },
                new Author
                {
                    Id = Guid.NewGuid(),
                    Name = "Петро Іванов",
                    Biography = "Історик, автор книг про середньовічну Європу.",
                    DateOfBirth = new DateTime(1968, 7, 30),
                    Citizenship = "Україна"
                },
                new Author
                {
                    Id = Guid.NewGuid(),
                    Name = "Олександр Мельник",
                    Biography = "Філософ, автор книг про етику та мораль.",
                    DateOfBirth = new DateTime(1979, 10, 10),
                    Citizenship = "Україна"
                }
            };

            modelBuilder.Entity<Author>().HasData(authors);



            var random = new Random();
            var discounts = bookIds.Select(bookId => new Discount
            {
                DiscountId = Guid.NewGuid(), 
                BookId = bookId,
                DiscountRate = random.Next(1, 36), 
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1)
            }).ToList();

            modelBuilder.Entity<Discount>().HasData(discounts);


            var books = new List<Book>
            {
                new Book
                {
                    Id = bookIds[0],
                    Title = "Місто зі скла",
                    AuthorId = authors[0].Id,
                    PublisherId = publishers[2].Id,
                    CategoryId = categories[0].Id,
                    Price = 350.99f,
                    Language = Language.UKRAINIAN,
                    Year = new DateTime(2023, 1, 1),
                    Description = "Фантастичний роман про місто, побудоване зі скла.",
                    Cover = CoverType.HARDCOVER,
                    Quantity = 5,
                    AudioFileUrl = await filesHelper.UploadAudioAsync(localAudioPath, bookIds[0]),
                    ImageUrl = await filesHelper.UploadImageAsync(imagePaths["Місто зі скла"], bookIds[0]),
                    DiscountId = discounts[0].DiscountId,
                    PdfFileUrl = await filesHelper.UploadPdfAsync(pdflocalAudioPath, bookIds[0]),
                },
                new Book
                {
                    Id = bookIds[1],
                    Title = "Тіні минулого",
                    AuthorId = authors[1].Id,
                    PublisherId = publishers[1].Id,
                    CategoryId = categories[1].Id,
                    Price = 250.99f,
                    Language = Language.UKRAINIAN,
                    Year = new DateTime(2022, 1, 1),
                    Description = "Детективний роман з несподіваною розв'язкою.",
                    Cover = CoverType.SOFT_COVER,
                    Quantity = 3,
                    DiscountId = discounts[1].DiscountId,
                    AudioFileUrl = await filesHelper.UploadAudioAsync(localAudioPath, bookIds[1]),
                    ImageUrl = await filesHelper.UploadImageAsync(imagePaths["Тіні минулого"], bookIds[1]),
                    PdfFileUrl = await filesHelper.UploadPdfAsync(pdflocalAudioPath, bookIds[1]),
                },
                new Book
                {
                    Id = bookIds[2],
                    Title = "Емоційний інтелект",
                    AuthorId = authors[2].Id,
                    PublisherId = publishers[4].Id,
                    CategoryId = categories[4].Id,
                    Price = 400.99f,
                    Language = Language.UKRAINIAN,
                    Year = new DateTime(2021, 1, 1),
                    Description = "Книга про те, як розвивати емоційний інтелект.",
                    Cover = CoverType.HARDCOVER,
                    Quantity = 1,
                    DiscountId = discounts[2].DiscountId,
                    AudioFileUrl = await filesHelper.UploadAudioAsync(localAudioPath, bookIds[2]),
                    ImageUrl = await filesHelper.UploadImageAsync(imagePaths["Емоційний інтелект"], bookIds[2]),
                    PdfFileUrl = await filesHelper.UploadPdfAsync(pdflocalAudioPath, bookIds[2]),
                }
            };
            modelBuilder.Entity<Book>().HasData(books);


            modelBuilder.Entity("BookSubCategory").HasData(
                new { BookId = books[0].Id, SubCategoryId = subCategories[0].Id },
                new { BookId = books[1].Id, SubCategoryId = subCategories[3].Id },
                new { BookId = books[2].Id, SubCategoryId = subCategories[12].Id }
            );

            var feedbacks = new List<Feedback>
            {
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    Comment = "Чудова книга! Захоплюючий сюжет.",
                    Rating = 5,
                    Date = DateTime.UtcNow,
                    IsPurchased = true,
                    BookId = books[0].Id,
                    UserId = new Guid("69be6ab0-0ad0-4ac9-bcce-096ebfa9bb4c")


                },
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    Comment = "Цікава книга, але кінець трохи розчарував.",
                    Rating = 4,
                    Date = DateTime.UtcNow,
                    IsPurchased = true,
                    BookId = books[1].Id,
                    UserId = new Guid("69be6ab0-0ad0-4ac9-bcce-096ebfa9bb4c")


                },
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    Comment = "Дуже корисна книга для саморозвитку.",
                    Rating = 5,
                    Date = DateTime.UtcNow,
                    IsPurchased = true,
                    BookId = books[2].Id,
                    UserId = new Guid("69be6ab0-0ad0-4ac9-bcce-096ebfa9bb4c")

                }
            };
            modelBuilder.Entity<Feedback>().HasData(feedbacks);
        }


    }
}
