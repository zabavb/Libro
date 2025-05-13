
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


            // 60 static ids for books
            // to add more but automatically use
            // Enumerable.Range(0, n).Select(_ => Guid.NewGuid()).ToList();
            List<Guid> bookIds = new List<Guid>
            {
                new("df76d28a-1309-4d04-86cc-58eccd373ba3"),
                new("336618f9-fd33-4775-90ab-c6e895f60b0a"),
                new("05f3ed9d-05aa-4421-addf-e85906260f64"),
                new("d0fa5452-d1c7-420d-b0ed-0e7feb55ec4f"),
                new("a943fd38-91bc-42de-8316-f67ca4d292db"),
                new("99e2aefe-561e-4de9-8ddc-581b24d49a17"),
                new("0c322dd3-ff92-4a55-ba5a-fc27f00d9e6e"),
                new("7869777a-ae20-417d-8574-1b6800e0608f"),
                new("d7250d33-4736-4d7d-997f-dc445d47c7ce"),
                new("a0e8c26e-be8c-40d3-8240-cd1c67e3fd08"),
                new("9d80eb19-a069-4267-befe-9ffd1959312e"),
                new("048e793f-e796-40a1-b535-7fae46d0cbc6"),
                new("b1858dc5-4ea0-431e-ae56-d23f75b16830"),
                new("5f99ca9e-4115-4ed8-8fad-b2d7d9609151"),
                new("582d1877-1703-445f-afa0-55001225d4fb"),
                new("a56e4018-dfc7-410c-a670-e68fb2a337c9"),
                new("8448f8d1-acfa-4e86-a395-c4c2ad8076bd"),
                new("7d8ce6cd-a68b-434a-97d1-77ea4a1c099e"),
                new("1cf76a75-54d8-478c-8d22-3d22811f880a"),
                new("64ce1687-3624-4f97-80e4-142e216f3c5d"),
                new("9f985d40-16a4-46d8-91c8-d266e18c92c9"),
                new("398fff8e-7803-46a3-b72e-25b9b56b8789"),
                new("52b6b0ff-51d9-46a8-b684-f18ef295f7e6"),
                new("f22e076e-1924-4296-979c-41e5a6111ce6"),
                new("2c67597b-9900-485d-a557-98df02e16c88"),
                new("72832bdc-41be-41a2-a1cb-05942832e8f2"),
                new("142f24c3-271a-4d70-bab4-be0612290e80"),
                new("8557babd-db43-4530-b69d-75beaf347f04"),
                new("37dfbb14-b3fc-4d39-ac02-58b2a438a7cf"),
                new("9b0a735f-593e-41fb-ba99-25d7631d792d"),
                new("1a54e86a-e5c2-41ab-9ac3-0246b17db4bd"),
                new("2a6bab6e-7324-4914-9d23-b26726762a1d"),
                new("553b2b85-cc6e-4dad-b468-8486f5be9a8f"),
                new("dec7d818-5875-4953-9684-31cf7aa205b5"),
                new("dfb29769-25b1-4c8c-9da4-31322d620322"),
                new("edb367dd-3a49-4094-94b4-0425e09ae529"),
                new("1b0fbde9-5737-4e29-a5ba-b84b3b9e793d"),
                new("e50a4c5b-83c1-4b2e-b86c-359caf8c081c"),
                new("7bc7a60d-ea12-463f-b20c-a8213bbda7e1"),
                new("af053ef5-91cb-4d65-ac43-89436bbe718d"),
                new("5baf5039-6c47-4601-ba4e-9bbf58412b67"),
                new("57449f3a-82ab-4e37-bbfc-927b8638fe50"),
                new("a47b649a-8020-4f9d-9133-8074253c9a50"),
                new("b9a52a11-c3fb-41c8-a692-3e870aa584a9"),
                new("188f16a9-07f1-4787-87e4-cad25a653fbc"),
                new("3dd29e5f-a0bb-4a9d-a693-16fa09dfd32d"),
                new("4757fed2-bb94-437f-8690-490acc7aa3db"),
                new("2f50840c-bcb5-4ac6-bb31-e139dc278321"),
                new("5f93ba8a-1b74-42a4-9066-d92791158d3f"),
                new("8ec71ef7-8592-4f57-9282-f73ad302b0e9"),
                new("520f1e61-341b-45fa-a93c-afaec6171199"),
                new("c8353ff3-ef62-4682-9905-e33249742474"),
                new("eea2634b-b39a-4b1a-bb69-0a35d07f0e81"),
                new("379b5533-22f7-4a76-80f7-0248577e14eb"),
                new("622f712c-3368-4806-b4a3-756d3681dd8c"),
                new("23c98993-bbae-41df-936f-2e61fedbf357"),
                new("516bf050-80f3-4507-b0a7-9a16615e28a9"),
                new("3e5d8ce0-299c-4e5d-a0e8-1a7c1872b425"),
                new("a130fbc1-d480-49c2-8560-3bd358c636b6"),
                new("87adae37-1b77-4f00-91ee-28b1733fc514")
            };


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
