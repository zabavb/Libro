
using Microsoft.EntityFrameworkCore;
using BookAPI.Models;
using Library.Common;
using Library.DTOs.Book;
using Author = BookAPI.Models.Author;
using Book = BookAPI.Models.Book;
using Category = BookAPI.Models.Category;
using Discount = BookAPI.Models.Discount;
using Feedback = BookAPI.Models.Feedback;
using Publisher = BookAPI.Models.Publisher;
using SubCategory = BookAPI.Models.SubCategory;

namespace BookAPI.Data
{
    public static class DataSeeder
    {
        public static async Task Seed(ModelBuilder modelBuilder, S3StorageService storageService)
        {
            var filesHelper = new FilesHelper(storageService, "libro-book");

            var bookIds = Enumerable.Range(0, 15).Select(_ => Guid.NewGuid()).ToList();

            var imagePaths = new Dictionary<string, string>
            {
                { "Місто зі скла", "Data/Files/Images/місто_зі_скла.jpeg" },
                { "Тіні минулого", "Data/Files/Images/тіні_минулого.jpeg" },
                { "Емоційний інтелект", "Data/Files/Images/емоційний_інтелект.jpeg" },
                { "У пошуках світла", "Data/Files/Images/у_пошуках_світла.jpeg" },
                { "Поза межами розуму", "Data/Files/Images/поза_межами_розуму.jpeg" },
                { "Код надії", "Data/Files/Images/код_надії.jpeg" },
                { "Таємниця лісу", "Data/Files/Images/таємниця_лісу.jpeg" },
                { "Історії з майбутнього", "Data/Files/Images/історії_з_майбутнього.jpeg" },
                { "Психологія впливу", "Data/Files/Images/психологія_впливу.jpeg" },
                { "Книга мандрівника", "Data/Files/Images/книга_мандрівника.jpeg" },
                { "Фізика для допитливих", "Data/Files/Images/фізика_для_допитливих.jpeg" },
                { "Біологія життя", "Data/Files/Images/біологія_життя.jpeg" },
                { "Магія свідомості", "Data/Files/Images/магія_свідомості.jpeg" },
                { "Кохання у віртуальному світі", "Data/Files/Images/кохання_у_віртуальному_світі.jpeg" },
                { "Всесвіт всередині нас", "Data/Files/Images/всесвіт_всередині_нас.jpeg" }
            };


            var pdfPaths = new Dictionary<string, string>
            {
                { "Місто зі скла", "Data/Files/Pdfs/місто_зі_скла.pdf" },
                { "Тіні минулого", "Data/Files/Pdfs/тіні_минулого.pdf" },
                { "Емоційний інтелект", "Data/Files/Pdfs/емоційний_інтелект.pdf" },
                { "У пошуках світла", "Data/Files/Pdfs/у_пошуках_світла.pdf" },
                { "Поза межами розуму", "Data/Files/Pdfs/поза_межами_розуму.pdf" },
                { "Код надії", "Data/Files/Pdfs/код_надії.pdf" },
                { "Таємниця лісу", "Data/Files/Pdfs/таємниця_лісу.pdf" },
                { "Історії з майбутнього", "Data/Files/Pdfs/історії_з_майбутнього.pdf" },
                { "Психологія впливу", "Data/Files/Pdfs/психологія_впливу.pdf" },
                { "Книга мандрівника", "Data/Files/Pdfs/книга_мандрівника.pdf" },
                { "Фізика для допитливих", "Data/Files/Pdfs/фізика_для_допитливих.pdf" },
                { "Біологія життя", "Data/Files/Pdfs/біологія_життя.pdf" },
                { "Магія свідомості", "Data/Files/Pdfs/магія_свідомості.pdf" },
                { "Кохання у віртуальному світі", "Data/Files/Pdfs/кохання_у_віртуальному_світі.pdf" },
                { "Всесвіт всередині нас", "Data/Files/Pdfs/всесвіт_всередині_нас.pdf" }
            };

            var audioPaths = new Dictionary<string, string>
            {
                { "Місто зі скла", "Data/Files/Audios/місто_зі_скла.mp3" },
                { "Тіні минулого", "Data/Files/Audios/тіні_минулого.mp3" },
                { "Емоційний інтелект", "Data/Files/Audios/емоційний_інтелект.mp3" },
                { "У пошуках світла", "Data/Files/Audios/у_пошуках_світла.mp3" },
                { "Поза межами розуму", "Data/Files/Audios/поза_межами_розуму.mp3" },
                { "Код надії", "Data/Files/Audios/код_надії.mp3" },
                { "Таємниця лісу", "Data/Files/Audios/таємниця_лісу.mp3" },
                { "Історії з майбутнього", "Data/Files/Audios/історії_з_майбутнього.mp3" },
                { "Психологія впливу", "Data/Files/Audios/психологія_впливу.mp3" },
                { "Книга мандрівника", "Data/Files/Audios/книга_мандрівника.mp3" },
                { "Фізика для допитливих", "Data/Files/Audios/фізика_для_допитливих.mp3" },
                { "Біологія життя", "Data/Files/Audios/біологія_життя.mp3" },
                { "Магія свідомості", "Data/Files/Audios/магія_свідомості.mp3" },
                { "Кохання у віртуальному світі", "Data/Files/Audios/кохання_у_віртуальному_світі.mp3" },
                { "Всесвіт всередині нас", "Data/Files/Audios/всесвіт_всередині_нас.mp3" }
            };
            var authorImagePaths = new Dictionary<string, string>
            {
                { "Джон Сміт", "Data/Files/Images/джон_сміт.jpeg" },
                { "Анна Браун", "Data/Files/Images/анна_браун.jpeg" },
                { "Марія Коваль", "Data/Files/Images/марія_коваль.jpeg" },
                { "Петро Іванов", "Data/Files/Images/петро_іванов.jpeg" },
                { "Олександр Мельник", "Data/Files/Images/олександр_мельник.jpeg" }
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
                    Citizenship = "Україна",
                }
            };
            foreach (var author in authors)
            {
                var imageFilePath = authorImagePaths[author.Name];
                var imageFile = filesHelper.GetFormFileFromPath(imageFilePath, "image/jpeg");
                var imageUrl = await filesHelper.UploadImageFromFormAsync(imageFile, author.Id, GlobalConstants.authorFolderImage);
                author.ImageUrl = imageUrl;
            }

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
                    DiscountId = discounts[0].DiscountId,
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
                    Cover = CoverType.SOFTCOVER,
                    Quantity = 3,
                    DiscountId = discounts[1].DiscountId,
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
                },
                new Book
                 {
                     Id = bookIds[3],
                     Title = "У пошуках світла",
                     AuthorId = authors[3].Id,
                     PublisherId = publishers[0].Id,
                     CategoryId = categories[3].Id,
                     Price = 299.99f,
                     Language = Language.UKRAINIAN,
                     Year = new DateTime(2020, 1, 1),
                     Description = "Натхненна історія про подолання труднощів.",
                     Cover = CoverType.SOFTCOVER,
                     Quantity = 2,
                     DiscountId = discounts[3].DiscountId,
                 },
                 new Book
                 {
                     Id = bookIds[4],
                     Title = "Поза межами розуму",
                     AuthorId = authors[4].Id,
                     PublisherId = publishers[3].Id,
                     CategoryId = categories[4].Id,
                     Price = 370.00f,
                     Language = Language.ENGLISH,
                     Year = new DateTime(2021, 5, 10),
                     Description = "Науково-популярна книга про можливості людського мозку.",
                     Cover = CoverType.HARDCOVER,
                     Quantity = 4,
                     DiscountId = discounts[4].DiscountId,
                 },
                 new Book
                 {
                     Id = bookIds[5],
                     Title = "Код надії",
                     AuthorId = authors[2].Id,
                     PublisherId = publishers[2].Id,
                     CategoryId = categories[0].Id,
                     Price = 310.00f,
                     Language = Language.UKRAINIAN,
                     Year = new DateTime(2022, 11, 15),
                     Description = "Фантастична пригода у постапокаліптичному світі.",
                     Cover = CoverType.SOFTCOVER,
                     Quantity = 7,
                     DiscountId = discounts[0].DiscountId,
                 },
                 new Book
                 {
                     Id = bookIds[6],
                     Title = "Таємниця лісу",
                     AuthorId = authors[1].Id,
                     PublisherId = publishers[1].Id,
                     CategoryId = categories[2].Id,
                     Price = 260.00f,
                     Language = Language.UKRAINIAN,
                     Year = new DateTime(2019, 3, 7),
                     Description = "Детектив у стилі нуар серед дикої природи.",
                     Cover = CoverType.SOFTCOVER,
                     Quantity = 3,
                     DiscountId = discounts[1].DiscountId,
                 },
                 new Book
                 {
                     Id = bookIds[7],
                     Title = "Історії з майбутнього",
                     AuthorId = authors[0].Id,
                     PublisherId = publishers[0].Id,
                     CategoryId = categories[0].Id,
                     Price = 390.00f,
                     Language = Language.ENGLISH,
                     Year = new DateTime(2024, 2, 20),
                     Description = "Збірка оповідань про альтернативні реальності.",
                     Cover = CoverType.HARDCOVER,
                     Quantity = 6,
                     DiscountId = discounts[3].DiscountId,
                 },
                 new Book
                 {
                     Id = bookIds[8],
                     Title = "Психологія впливу",
                     AuthorId = authors[2].Id,
                     PublisherId = publishers[4].Id,
                     CategoryId = categories[4].Id,
                     Price = 330.00f,
                     Language = Language.UKRAINIAN,
                     Year = new DateTime(2020, 6, 1),
                     Description = "Практичне керівництво по впливу на людей.",
                     Cover = CoverType.SOFTCOVER,
                     Quantity = 5,
                     DiscountId = discounts[2].DiscountId,
                 },
                 new Book
                 {
                     Id = bookIds[9],
                     Title = "Книга мандрівника",
                     AuthorId = authors[3].Id,
                     PublisherId = publishers[2].Id,
                     CategoryId = categories[3].Id,
                     Price = 280.00f,
                     Language = Language.UKRAINIAN,
                     Year = new DateTime(2018, 7, 15),
                     Description = "Путівник та мотиваційна література для мандрівників.",
                     Cover = CoverType.SOFTCOVER,
                     Quantity = 2,
                     DiscountId = discounts[4].DiscountId,
                 },
                 new Book
                 {
                     Id = bookIds[10],
                     Title = "Фізика для допитливих",
                     AuthorId = authors[4].Id,
                     PublisherId = publishers[3].Id,
                     CategoryId = categories[5].Id,
                     Price = 310.50f,
                     Language = Language.UKRAINIAN,
                     Year = new DateTime(2021, 9, 5),
                     Description = "Легкий вступ до складних фізичних понять.",
                     Cover = CoverType.HARDCOVER,
                     Quantity = 4,
                     DiscountId = discounts[1].DiscountId,
                 },
                 new Book
                 {
                     Id = bookIds[11],
                     Title = "Біологія життя",
                     AuthorId = authors[4].Id,
                     PublisherId = publishers[3].Id,
                     CategoryId = categories[5].Id,
                     Price = 299.99f,
                     Language = Language.ENGLISH,
                     Year = new DateTime(2023, 4, 12),
                     Description = "Ілюстроване видання для вивчення біології.",
                     Cover = CoverType.SOFTCOVER,
                     Quantity = 6,
                     DiscountId = discounts[3].DiscountId,
                 },
                 new Book
                 {
                     Id = bookIds[12],
                     Title = "Магія свідомості",
                     AuthorId = authors[2].Id,
                     PublisherId = publishers[4].Id,
                     CategoryId = categories[4].Id,
                     Price = 355.75f,
                     Language = Language.UKRAINIAN,
                     Year = new DateTime(2020, 10, 30),
                     Description = "Як мозок формує нашу реальність.",
                     Cover = CoverType.HARDCOVER,
                     Quantity = 1,
                     DiscountId = discounts[2].DiscountId,
                 },
                 new Book
                 {
                     Id = bookIds[13],
                     Title = "Кохання у віртуальному світі",
                     AuthorId = authors[1].Id,
                     PublisherId = publishers[1].Id,
                     CategoryId = categories[1].Id,
                     Price = 275.99f,
                     Language = Language.UKRAINIAN,
                     Year = new DateTime(2023, 8, 8),
                     Description = "Сучасна історія кохання у цифрову епоху.",
                     Cover = CoverType.SOFTCOVER,
                     Quantity = 3,
                     DiscountId = discounts[1].DiscountId,
                 },
                 new Book
                 {
                     Id = bookIds[14],
                     Title = "Всесвіт всередині нас",
                     AuthorId = authors[0].Id,
                     PublisherId = publishers[2].Id,
                     CategoryId = categories[5].Id,
                     Price = 380.00f,
                     Language = Language.UKRAINIAN,
                     Year = new DateTime(2019, 12, 25),
                     Description = "Наукові відкриття про людське тіло та свідомість.",
                     Cover = CoverType.HARDCOVER,
                     Quantity = 5,
                     DiscountId = discounts[4].DiscountId,
                 }
            };

            foreach (var book in books)
            {
                var imageFilePath = imagePaths[book.Title];
                var pdfFilePath = pdfPaths[book.Title];
                var audioFilePath = audioPaths[book.Title];

                var imageFile = filesHelper.GetFormFileFromPath(imageFilePath, "image/jpeg");
                var pdfFile = filesHelper.GetFormFileFromPath(pdfFilePath, "application/pdf");
                var audioFile = filesHelper.GetFormFileFromPath(audioFilePath, "audio/mpeg");

                var imageUrl = await filesHelper.UploadImageFromFormAsync(imageFile, book.Id, GlobalConstants.booksFolderImage);
                var pdfUrl = await filesHelper.UploadPdfFromFormAsync(pdfFile, book.Id, GlobalConstants.booksFolderPdf);
                var audioUrl = await filesHelper.UploadAudioFromFormAsync(audioFile, book.Id, GlobalConstants.booksFolderAudio);

                book.ImageUrl = imageUrl;
                book.PdfFileUrl = pdfUrl;
                book.AudioFileUrl = audioUrl;
            }
            modelBuilder.Entity<Book>().HasData(books);


            modelBuilder.Entity("BookSubCategory").HasData(
                // Місто зі скла – фантастика
                new { BookId = books[0].Id, SubCategoryId = subCategories[0].Id }, // Космічна фантастика
                new { BookId = books[0].Id, SubCategoryId = subCategories[1].Id }, // Фентезі

                // Тіні минулого – детектив
                new { BookId = books[1].Id, SubCategoryId = subCategories[3].Id }, // Кримінальний детектив
                new { BookId = books[1].Id, SubCategoryId = subCategories[4].Id }, // Трилер

                // Емоційний інтелект – психологія
                new { BookId = books[2].Id, SubCategoryId = subCategories[12].Id }, // Емоційний інтелект

                // У пошуках світла – історія
                new { BookId = books[3].Id, SubCategoryId = subCategories[9].Id }, // Стародавній світ

                // Поза межами розуму – психологія
                new { BookId = books[4].Id, SubCategoryId = subCategories[13].Id }, // Саморозвиток

                // Код надії – фантастика
                new { BookId = books[5].Id, SubCategoryId = subCategories[2].Id }, // Альтернативна історія

                // Таємниця лісу – детектив
                new { BookId = books[6].Id, SubCategoryId = subCategories[5].Id }, // Поліцейський детектив

                // Історії з майбутнього – фантастика
                new { BookId = books[7].Id, SubCategoryId = subCategories[0].Id }, // Космічна фантастика

                // Психологія впливу – психологія
                new { BookId = books[8].Id, SubCategoryId = subCategories[14].Id }, // Психологія стосунків

                // Книга мандрівника – історія
                new { BookId = books[9].Id, SubCategoryId = subCategories[10].Id }, // Середньовіччя

                // Фізика для допитливих – наукова література
                new { BookId = books[10].Id, SubCategoryId = subCategories[6].Id }, // Фізика

                // Біологія життя – наукова література
                new { BookId = books[11].Id, SubCategoryId = subCategories[7].Id }, // Біологія

                // Магія свідомості – психологія
                new { BookId = books[12].Id, SubCategoryId = subCategories[12].Id }, // Емоційний інтелект

                // Кохання у віртуальному світі – детектив
                new { BookId = books[13].Id, SubCategoryId = subCategories[4].Id }, // Трилер

                // Всесвіт всередині нас – наукова література
                new { BookId = books[14].Id, SubCategoryId = subCategories[8].Id } // Астрономія
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
