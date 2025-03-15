using Microsoft.EntityFrameworkCore;
using BookAPI.Models;
using System;
using System.Collections.Generic;

namespace BookAPI.Data
{
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Додавання категорій
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

            // Додавання книг
            var books = new List<Book>
            {
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Місто зі скла",
                    AuthorId = authors[0].Id,
                    PublisherId = publishers[2].Id,
                    CategoryId = categories[0].Id,
                    Price = 350.99f,
                    Language = Language.UKRAINIAN,
                    Year = new DateTime(2023, 1, 1),
                    Description = "Фантастичний роман про місто, побудоване зі скла.",
                    Cover = CoverType.HARDCOVER,
                    IsAvaliable = true
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Тіні минулого",
                    AuthorId = authors[1].Id,
                    PublisherId = publishers[1].Id,
                    CategoryId = categories[1].Id,
                    Price = 250.99f,
                    Language = Language.UKRAINIAN,
                    Year = new DateTime(2022, 1, 1),
                    Description = "Детективний роман з несподіваною розв'язкою.",
                    Cover = CoverType.SOFT_COVER,
                    IsAvaliable = true
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Емоційний інтелект",
                    AuthorId = authors[2].Id,
                    PublisherId = publishers[4].Id,
                    CategoryId = categories[4].Id,
                    Price = 400.99f,
                    Language = Language.UKRAINIAN,
                    Year = new DateTime(2021, 1, 1),
                    Description = "Книга про те, як розвивати емоційний інтелект.",
                    Cover = CoverType.HARDCOVER,
                    IsAvaliable = true
                }
            };
            modelBuilder.Entity<Book>().HasData(books);

            // Додавання зв'язків між книгами та підкатегоріями
            modelBuilder.Entity("BookSubCategory").HasData(
                new { BookId = books[0].Id, SubCategoryId = subCategories[0].Id }, // Місто зі скла -> Космічна фантастика
                new { BookId = books[1].Id, SubCategoryId = subCategories[3].Id }, // Тіні минулого -> Кримінальний детектив
                new { BookId = books[2].Id, SubCategoryId = subCategories[12].Id } // Емоційний інтелект -> Емоційний інтелект
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
    }
}