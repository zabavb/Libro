//using Microsoft.EntityFrameworkCore;
//using BookAPI.Models;
//using System;
//using System.Collections.Generic;

//namespace BookAPI.Data
//{
//    public static class DataSeeder
//    {
//        public static void Seed(ModelBuilder modelBuilder)
//        {
//            // Додавання категорій
//            var categories = new List<Category>
//            {
//                new Category { Id = Guid.NewGuid(), Name = "Фантастика" },
//                new Category { Id = Guid.NewGuid(), Name = "Детектив" },
//                new Category { Id = Guid.NewGuid(), Name = "Наукова література" },
//                new Category { Id = Guid.NewGuid(), Name = "Історія" },
//                new Category { Id = Guid.NewGuid(), Name = "Психологія" },
//                new Category { Id = Guid.NewGuid(), Name = "Філософія" },
//                new Category { Id = Guid.NewGuid(), Name = "Економіка" },
//                new Category { Id = Guid.NewGuid(), Name = "Мистецтво" },
//                new Category { Id = Guid.NewGuid(), Name = "Подорожі" },
//                new Category { Id = Guid.NewGuid(), Name = "Кулінарія" }
//            };
//            modelBuilder.Entity<Category>().HasData(categories);

//            // Додавання підкатегорій
//            var subCategories = new List<SubCategory>
//            {
//                // Підкатегорії для "Фантастика"
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Name = "Космічна фантастика" },
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Name = "Фентезі" },
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[0].Id, Name = "Альтернативна історія" },

//                // Підкатегорії для "Детектив"
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[1].Id, Name = "Кримінальний детектив" },
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[1].Id, Name = "Трилер" },
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[1].Id, Name = "Поліцейський детектив" },

//                // Підкатегорії для "Наукова література"
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[2].Id, Name = "Фізика" },
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[2].Id, Name = "Біологія" },
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[2].Id, Name = "Астрономія" },

//                // Підкатегорії для "Історія"
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[3].Id, Name = "Стародавній світ" },
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[3].Id, Name = "Середньовіччя" },
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[3].Id, Name = "Нова історія" },

//                // Підкатегорії для "Психологія"
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[4].Id, Name = "Емоційний інтелект" },
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[4].Id, Name = "Саморозвиток" },
//                new SubCategory { Id = Guid.NewGuid(), CategoryId = categories[4].Id, Name = "Психологія стосунків" }
//            };
//            modelBuilder.Entity<SubCategory>().HasData(subCategories);

//            // Додавання видавництв
//            var publishers = new List<Publisher>
//            {
//                new Publisher { Id = Guid.NewGuid(), Name = "Видавництво А", Description = "Видавництво, що спеціалізується на науковій літературі." },
//                new Publisher { Id = Guid.NewGuid(), Name = "Видавництво Б", Description = "Видавництво, відоме своїми детективами та трилерами." },
//                new Publisher { Id = Guid.NewGuid(), Name = "Видавництво В", Description = "Видавництво, що видає книги з фантастики та фентезі." },
//                new Publisher { Id = Guid.NewGuid(), Name = "Видавництво Г", Description = "Видавництво, що спеціалізується на історичних книгах." },
//                new Publisher { Id = Guid.NewGuid(), Name = "Видавництво Д", Description = "Видавництво, що видає книги з психології та саморозвитку." }
//            };
//            modelBuilder.Entity<Publisher>().HasData(publishers);

//            // Додавання авторів
//            var authors = new List<Author>
//            {
//                new Author { Id = Guid.NewGuid(), Name = "Джон Сміт", Biography = "Відомий письменник у жанрі фантастики, автор бестселерів." },
//                new Author { Id = Guid.NewGuid(), Name = "Анна Браун", Biography = "Авторка детективних романів, відома своїми захоплюючими сюжетами." },
//                new Author { Id = Guid.NewGuid(), Name = "Марія Коваль", Biography = "Психологиня, авторка книг про емоційний інтелект та стрес." },
//                new Author { Id = Guid.NewGuid(), Name = "Петро Іванов", Biography = "Історик, автор книг про середньовічну Європу." },
//                new Author { Id = Guid.NewGuid(), Name = "Олександр Мельник", Biography = "Філософ, автор книг про етику та мораль." }
//            };
//            modelBuilder.Entity<Author>().HasData(authors);

//            // Додавання книг
//            var books = new List<Book>
//            {
//                new Book
//                {
//                    Id = Guid.NewGuid(),
//                    Title = "Місто зі скла",
//                    AuthorId = authors[0].Id,
//                    PublisherId = publishers[2].Id,
//                    CategoryId = categories[0].Id,
//                    Price = 350.99f,
//                    Language = Language.UKRAINIAN,
//                    Year = new DateTime(2023, 1, 1),
//                    Description = "Фантастичний роман про місто, побудоване зі скла.",
//                    Cover = CoverType.HARDCOVER,
//                    IsAvaliable = true
//                },
//                new Book
//                {
//                    Id = Guid.NewGuid(),
//                    Title = "Тіні минулого",
//                    AuthorId = authors[1].Id,
//                    PublisherId = publishers[1].Id,
//                    CategoryId = categories[1].Id,
//                    Price = 250.99f,
//                    Language = Language.UKRAINIAN,
//                    Year = new DateTime(2022, 1, 1),
//                    Description = "Детективний роман з несподіваною розв'язкою.",
//                    Cover = CoverType.SOFT_COVER,
//                    IsAvaliable = true
//                },
//                new Book
//                {
//                    Id = Guid.NewGuid(),
//                    Title = "Емоційний інтелект",
//                    AuthorId = authors[2].Id,
//                    PublisherId = publishers[4].Id,
//                    CategoryId = categories[4].Id,
//                    Price = 400.99f,
//                    Language = Language.UKRAINIAN,
//                    Year = new DateTime(2021, 1, 1),
//                    Description = "Книга про те, як розвивати емоційний інтелект.",
//                    Cover = CoverType.HARDCOVER,
//                    IsAvaliable = true
//                }
//            };
//            modelBuilder.Entity<Book>().HasData(books);

//            // Додавання зв'язків між книгами та підкатегоріями
//            modelBuilder.Entity("BookSubCategory").HasData(
//                new { BookId = books[0].Id, SubCategoryId = subCategories[0].Id }, // Місто зі скла -> Космічна фантастика
//                new { BookId = books[1].Id, SubCategoryId = subCategories[3].Id }, // Тіні минулого -> Кримінальний детектив
//                new { BookId = books[2].Id, SubCategoryId = subCategories[12].Id } // Емоційний інтелект -> Емоційний інтелект
//            );

//            // Додавання відгуків
//            var feedbacks = new List<Feedback>
//            {
//                new Feedback
//                {
//                    Id = Guid.NewGuid(),
//                    ReviewerName = "Іван",
//                    Comment = "Чудова книга! Захоплюючий сюжет.",
//                    Rating = 5,
//                    Date = DateTime.UtcNow,
//                    IsPurchased = true,
//                    BookId = books[0].Id
//                },
//                new Feedback
//                {
//                    Id = Guid.NewGuid(),
//                    ReviewerName = "Ольга",
//                    Comment = "Цікава книга, але кінець трохи розчарував.",
//                    Rating = 4,
//                    Date = DateTime.UtcNow,
//                    IsPurchased = true,
//                    BookId = books[1].Id
//                },
//                new Feedback
//                {
//                    Id = Guid.NewGuid(),
//                    ReviewerName = "Марія",
//                    Comment = "Дуже корисна книга для саморозвитку.",
//                    Rating = 5,
//                    Date = DateTime.UtcNow,
//                    IsPurchased = true,
//                    BookId = books[2].Id
//                }
//            };
//            modelBuilder.Entity<Feedback>().HasData(feedbacks);
//        }
//    }
//}


using Microsoft.EntityFrameworkCore;
using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Amazon.Runtime.Internal.Transform;
using Library.Interfaces;
using Library.Common;

namespace BookAPI.Data
{
    public static class DataSeeder
    {
        public static async Task Seed(ModelBuilder modelBuilder, S3StorageService storageService)
        {
            var bookIds = new List<Guid>
            {
                Guid.NewGuid(), // Id для "Місто зі скла"
                Guid.NewGuid(), // Id для "Тіні минулого"
                Guid.NewGuid()  // Id для "Емоційний інтелект"
            };


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

            // Додавання видавництв
            var publishers = new List<Publisher>
            {
                new Publisher { Id = Guid.NewGuid(), Name = "Видавництво А", Description = "Видавництво, що спеціалізується на науковій літературі." },
                new Publisher { Id = Guid.NewGuid(), Name = "Видавництво Б", Description = "Видавництво, відоме своїми детективами та трилерами." },
                new Publisher { Id = Guid.NewGuid(), Name = "Видавництво В", Description = "Видавництво, що видає книги з фантастики та фентезі." },
                new Publisher { Id = Guid.NewGuid(), Name = "Видавництво Г", Description = "Видавництво, що спеціалізується на історичних книгах." },
                new Publisher { Id = Guid.NewGuid(), Name = "Видавництво Д", Description = "Видавництво, що видає книги з психології та саморозвитку." }
            };
            modelBuilder.Entity<Publisher>().HasData(publishers);

            // Додавання авторів
            var authors = new List<Author>
            {
                new Author { Id = Guid.NewGuid(), Name = "Джон Сміт", Biography = "Відомий письменник у жанрі фантастики, автор бестселерів." },
                new Author { Id = Guid.NewGuid(), Name = "Анна Браун", Biography = "Авторка детективних романів, відома своїми захоплюючими сюжетами." },
                new Author { Id = Guid.NewGuid(), Name = "Марія Коваль", Biography = "Психологиня, авторка книг про емоційний інтелект та стрес." },
                new Author { Id = Guid.NewGuid(), Name = "Петро Іванов", Biography = "Історик, автор книг про середньовічну Європу." },
                new Author { Id = Guid.NewGuid(), Name = "Олександр Мельник", Biography = "Філософ, автор книг про етику та мораль." }
            };
            modelBuilder.Entity<Author>().HasData(authors);

            // Creation of discounts

            var discountsIds = new List<Guid>
            {
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
            };

            var random = new Random();
            var discounts = bookIds.Select(bookId => new Discount
            {
                DiscountId = Guid.NewGuid(),
                BookId = bookId,
                DiscountRate = random.Next(0, 36),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1)
            }).ToList();
            var id = discountsIds[0];



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
                    IsAvaliable = true,
                    AudioFileUrl = await UploadAudioAsync(storageService, audios[0], bookIds[0]), 
                    ImageUrl = await UploadImageAsync(storageService, imagePaths["Місто зі скла"], bookIds[0]),
                    DiscountId = discountsIds[0]
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
                    IsAvaliable = true,
                    ImageUrl = await UploadImageAsync(storageService, imagePaths["Тіні минулого"], bookIds[1]) 
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
                    IsAvaliable = true,
                    ImageUrl = await UploadImageAsync(storageService, imagePaths["Емоційний інтелект"], bookIds[2]) 
                }
            };
            modelBuilder.Entity<Book>().HasData(books);

         
            modelBuilder.Entity("BookSubCategory").HasData(
                new { BookId = books[0].Id, SubCategoryId = subCategories[0].Id }, 
                new { BookId = books[1].Id, SubCategoryId = subCategories[3].Id }, 
                new { BookId = books[2].Id, SubCategoryId = subCategories[12].Id } 
            );

            // Додавання відгуків
            var feedbacks = new List<Feedback>
            {
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    ReviewerName = "Іван",
                    Comment = "Чудова книга! Захоплюючий сюжет.",
                    Rating = 5,
                    Date = DateTime.UtcNow,
                    IsPurchased = true,
                    BookId = books[0].Id
                },
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    ReviewerName = "Ольга",
                    Comment = "Цікава книга, але кінець трохи розчарував.",
                    Rating = 4,
                    Date = DateTime.UtcNow,
                    IsPurchased = true,
                    BookId = books[1].Id
                },
                new Feedback
                {
                    Id = Guid.NewGuid(),
                    ReviewerName = "Марія",
                    Comment = "Дуже корисна книга для саморозвитку.",
                    Rating = 5,
                    Date = DateTime.UtcNow,
                    IsPurchased = true,
                    BookId = books[2].Id
                }
            };
            modelBuilder.Entity<Feedback>().HasData(feedbacks);
        }

        private static async Task<string> UploadImageAsync(S3StorageService storageService, string imageUrl, Guid bookId)
        {
            using var httpClient = new HttpClient();

            try
            {
                // Завантажуємо зображення з інтернету
                var response = await httpClient.GetAsync(imageUrl);

                // Перевіряємо, чи успішно завантажено
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to download image from {imageUrl}. Status code: {response.StatusCode}");
                }

                // Отримуємо потік даних зображення
                var stream = await response.Content.ReadAsStreamAsync();

                // Створюємо об'єкт IFormFile для завантаження на S3
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(imageUrl))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = response.Content.Headers.ContentType?.ToString() ?? "image/jpeg"
                };

                // Завантажуємо на S3
                return await storageService.UploadAsync(file,"book/images/", bookId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to upload image from URL: {imageUrl}", ex);
            }
        }

        public static async Task<string> UploadAudioAsync(IS3StorageService storageService, string audioUrl, Guid bookId)
        {
            using var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.GetAsync(audioUrl);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to download audio from {audioUrl}. Status code: {response.StatusCode}");
                }

                var stream = await response.Content.ReadAsStreamAsync();

                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(audioUrl))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = response.Content.Headers.ContentType?.ToString() ?? "audio/mpeg"
                };

                return await storageService.UploadAsync(file, "book/audios/", bookId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to upload audio from URL: {audioUrl}", ex);
            }
        }

    }
}