using Microsoft.EntityFrameworkCore;
using BookApi.Models;

namespace BookApi.Data
{
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var category1 = new Category { Id = Guid.NewGuid(), Name = "Фантастика" };
            var category2 = new Category { Id = Guid.NewGuid(), Name = "Детектив" };
            var category3 = new Category { Id = Guid.NewGuid(), Name = "Наукова література" };

            modelBuilder.Entity<Category>().HasData(category1, category2, category3);

            var publisher1 = new Publisher { Id = Guid.NewGuid(), Name = "Видавництво А", Description = "Опис видавництва А" };
            var publisher2 = new Publisher { Id = Guid.NewGuid(), Name = "Видавництво Б", Description = "Опис видавництва Б" };

            modelBuilder.Entity<Publisher>().HasData(publisher1, publisher2);

            var author1 = new Author { Id = Guid.NewGuid(), Name = "Джон Сміт", Biography = "Відомий письменник у жанрі фантастики." };
            var author2 = new Author { Id = Guid.NewGuid(), Name = "Анна Браун", Biography = "Авторка детективних романів." };

            modelBuilder.Entity<Author>().HasData(author1, author2);

            var book1 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 1",
                AuthorId = author1.Id,
                PublisherId = publisher1.Id,
                CategoryId = category1.Id,
                Price = 299.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2023, 1, 1),
                Description = "Опис Книги 1",
                Cover = CoverType.HARDCOVER,
                IsAvaliable = true
            };

            var book2 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 2",
                AuthorId = author2.Id,
                PublisherId = publisher2.Id,
                CategoryId = category2.Id,
                Price = 199.99f,
                Language = Language.ENGLISH,
                Year = new DateTime(2022, 1, 1),
                Description = "Опис Книги 2",
                Cover = CoverType.SOFT_COVER,
                IsAvaliable = false
            };

            modelBuilder.Entity<Book>().HasData(book1, book2);

            var feedback1 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Іван",
                Comment = "Чудова книга!",
                Rating = 5,
                Date = DateTime.UtcNow,
                IsPurchased = true,
                BookId = book1.Id
            };

            var feedback2 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Ольга",
                Comment = "Не дуже сподобалась.",
                Rating = 3,
                Date = DateTime.UtcNow,
                IsPurchased = false,
                BookId = book2.Id
            };


            modelBuilder.Entity<Feedback>().HasData(feedback1, feedback2);

            var category4 = new Category { Id = Guid.NewGuid(), Name = "Мистецтво" };
            var category5 = new Category { Id = Guid.NewGuid(), Name = "Історія" };
            var category6 = new Category { Id = Guid.NewGuid(), Name = "Психологія" };

            modelBuilder.Entity<Category>().HasData(category4, category5, category6);

            var publisher3 = new Publisher { Id = Guid.NewGuid(), Name = "Видавництво В", Description = "Опис видавництва В" };
            var publisher4 = new Publisher { Id = Guid.NewGuid(), Name = "Видавництво Г", Description = "Опис видавництва Г" };

            modelBuilder.Entity<Publisher>().HasData(publisher3, publisher4);

            var author3 = new Author { Id = Guid.NewGuid(), Name = "Марія Коваль", Biography = "Авторка книг з психології." };
            var author4 = new Author { Id = Guid.NewGuid(), Name = "Петро Іванов", Biography = "Письменник історичних романів." };

            modelBuilder.Entity<Author>().HasData(author3, author4);


            var book3 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 3",
                AuthorId = author3.Id,
                PublisherId = publisher3.Id,
                CategoryId = category6.Id,
                Price = 249.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2024, 1, 1),
                Description = "Опис Книги 3",
                Cover = CoverType.HARDCOVER,
                IsAvaliable = true
            };

            var book4 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 4",
                AuthorId = author4.Id,
                PublisherId = publisher4.Id,
                CategoryId = category5.Id,
                Price = 179.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2021, 1, 1),
                Description = "Опис Книги 4",
                Cover = CoverType.SOFT_COVER,
                IsAvaliable = true
            };

            modelBuilder.Entity<Book>().HasData(book3, book4);

            var feedback3 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Марина",
                Comment = "Цікава книга, багато корисної інформації.",
                Rating = 4,
                Date = DateTime.UtcNow,
                IsPurchased = true,
                BookId = book3.Id
            };

            var feedback4 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Ігор",
                Comment = "Тема книги мене не дуже зацікавила.",
                Rating = 2,
                Date = DateTime.UtcNow,
                IsPurchased = false,
                BookId = book4.Id
            };

            modelBuilder.Entity<Feedback>().HasData(feedback3, feedback4);

            var category7 = new Category { Id = Guid.NewGuid(), Name = "Філософія" };
            var category8 = new Category { Id = Guid.NewGuid(), Name = "Економіка" };
            var category9 = new Category { Id = Guid.NewGuid(), Name = "Технічна література" };

            modelBuilder.Entity<Category>().HasData(category7, category8, category9);

            var publisher5 = new Publisher { Id = Guid.NewGuid(), Name = "Видавництво Д", Description = "Опис видавництва Д" };
            var publisher6 = new Publisher { Id = Guid.NewGuid(), Name = "Видавництво Е", Description = "Опис видавництва Е" };

            modelBuilder.Entity<Publisher>().HasData(publisher5, publisher6);

            var author5 = new Author { Id = Guid.NewGuid(), Name = "Михайло Чернов", Biography = "Автор книг з філософії." };
            var author6 = new Author { Id = Guid.NewGuid(), Name = "Оксана Дорош", Biography = "Авторка книг з економіки." };

            modelBuilder.Entity<Author>().HasData(author5, author6);

            var book5 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 5",
                AuthorId = author5.Id,
                PublisherId = publisher5.Id,
                CategoryId = category7.Id,
                Price = 399.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2025, 1, 1),
                Description = "Опис Книги 5",
                Cover = CoverType.HARDCOVER,
                IsAvaliable = true
            };

            var book6 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 6",
                AuthorId = author6.Id,
                PublisherId = publisher6.Id,
                CategoryId = category8.Id,
                Price = 299.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2020, 1, 1),
                Description = "Опис Книги 6",
                Cover = CoverType.SOFT_COVER,
                IsAvaliable = true
            };

            modelBuilder.Entity<Book>().HasData(book5, book6);

            var feedback5 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Сергій",
                Comment = "Цікава та змістовна книга!",
                Rating = 5,
                Date = DateTime.UtcNow,
                IsPurchased = true,
                BookId = book5.Id
            };

            var feedback6 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Наталя",
                Comment = "Книга змістовна, але не для всіх.",
                Rating = 4,
                Date = DateTime.UtcNow,
                IsPurchased = false,
                BookId = book6.Id
            };

            modelBuilder.Entity<Feedback>().HasData(feedback5, feedback6);

            var category10 = new Category { Id = Guid.NewGuid(), Name = "Мистецтво" };
            var category11 = new Category { Id = Guid.NewGuid(), Name = "Історія" };
            var category12 = new Category { Id = Guid.NewGuid(), Name = "Психологія" };
            var category13 = new Category { Id = Guid.NewGuid(), Name = "Подорожі" };
            var category14 = new Category { Id = Guid.NewGuid(), Name = "Кулінарія" };

            modelBuilder.Entity<Category>().HasData(category10, category11, category12, category13, category14);

            var publisher7 = new Publisher { Id = Guid.NewGuid(), Name = "Видавництво Ж", Description = "Опис видавництва Ж" };
            var publisher8 = new Publisher { Id = Guid.NewGuid(), Name = "Видавництво З", Description = "Опис видавництва З" };

            modelBuilder.Entity<Publisher>().HasData(publisher7, publisher8);

            var author7 = new Author { Id = Guid.NewGuid(), Name = "Іван Коваль", Biography = "Автор книги про мистецтво." };
            var author8 = new Author { Id = Guid.NewGuid(), Name = "Людмила Мельник", Biography = "Авторка історичних досліджень." };
            var author9 = new Author { Id = Guid.NewGuid(), Name = "Вікторія Орлова", Biography = "Психолог, авторка книг по психології." };
            var author10 = new Author { Id = Guid.NewGuid(), Name = "Юрій Харчук", Biography = "Подорожник та мандрівник." };
            var author11 = new Author { Id = Guid.NewGuid(), Name = "Наталія Гриценко", Biography = "Кулінарна блогерка, авторка книг з кулінарії." };

            modelBuilder.Entity<Author>().HasData(author7, author8, author9, author10, author11);

            var book7 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 7",
                AuthorId = author7.Id,
                PublisherId = publisher7.Id,
                CategoryId = category10.Id,
                Price = 499.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2024, 1, 1),
                Description = "Опис Книги 7",
                Cover = CoverType.HARDCOVER,
                IsAvaliable = true
            };

            var book8 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 8",
                AuthorId = author8.Id,
                PublisherId = publisher8.Id,
                CategoryId = category11.Id,
                Price = 399.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2023, 1, 1),
                Description = "Опис Книги 8",
                Cover = CoverType.SOFT_COVER,
                IsAvaliable = true
            };

            var book9 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 9",
                AuthorId = author9.Id,
                PublisherId = publisher7.Id,
                CategoryId = category12.Id,
                Price = 299.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2022, 1, 1),
                Description = "Опис Книги 9",
                Cover = CoverType.HARDCOVER,
                IsAvaliable = false
            };

            var book10 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 10",
                AuthorId = author10.Id,
                PublisherId = publisher8.Id,
                CategoryId = category13.Id,
                Price = 249.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2021, 1, 1),
                Description = "Опис Книги 10",
                Cover = CoverType.SOFT_COVER,
                IsAvaliable = true
            };

            var book11 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 11",
                AuthorId = author11.Id,
                PublisherId = publisher7.Id,
                CategoryId = category14.Id,
                Price = 199.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2020, 1, 1),
                Description = "Опис Книги 11",
                Cover = CoverType.HARDCOVER,
                IsAvaliable = true
            };

            modelBuilder.Entity<Book>().HasData(book7, book8, book9, book10, book11);

            var feedback7 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Михайло",
                Comment = "Вражаюче мистецтво!",
                Rating = 5,
                Date = DateTime.UtcNow,
                IsPurchased = true,
                BookId = book7.Id
            };

            var feedback8 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Олексій",
                Comment = "Інформативна книга з історії.",
                Rating = 4,
                Date = DateTime.UtcNow,
                IsPurchased = false,
                BookId = book8.Id
            };

            var feedback9 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Катерина",
                Comment = "Чудова книга по психології!",
                Rating = 5,
                Date = DateTime.UtcNow,
                IsPurchased = true,
                BookId = book9.Id
            };

            var feedback10 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Дмитро",
                Comment = "Чудові поради для мандрівників.",
                Rating = 4,
                Date = DateTime.UtcNow,
                IsPurchased = false,
                BookId = book10.Id
            };

            var feedback11 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Тетяна",
                Comment = "Смачні рецепти!",
                Rating = 5,
                Date = DateTime.UtcNow,
                IsPurchased = true,
                BookId = book11.Id
            };

            modelBuilder.Entity<Feedback>().HasData(feedback7, feedback8, feedback9, feedback10, feedback11);

            var category15 = new Category { Id = Guid.NewGuid(), Name = "Біографії" };
            var category16 = new Category { Id = Guid.NewGuid(), Name = "Філософія" };
            var category17 = new Category { Id = Guid.NewGuid(), Name = "Підручники" };
            var category18 = new Category { Id = Guid.NewGuid(), Name = "Дитячі книги" };
            var category19 = new Category { Id = Guid.NewGuid(), Name = "Поезія" };

            modelBuilder.Entity<Category>().HasData(category15, category16, category17, category18, category19);

            var publisher9 = new Publisher { Id = Guid.NewGuid(), Name = "Видавництво І", Description = "Опис видавництва І" };
            var publisher10 = new Publisher { Id = Guid.NewGuid(), Name = "Видавництво К", Description = "Опис видавництва К" };

            modelBuilder.Entity<Publisher>().HasData(publisher9, publisher10);


            var author12 = new Author { Id = Guid.NewGuid(), Name = "Марія Коваленко", Biography = "Авторка біографій видатних людей." };
            var author13 = new Author { Id = Guid.NewGuid(), Name = "Олександр Яценко", Biography = "Філософ, автор книг про мораль." };
            var author14 = new Author { Id = Guid.NewGuid(), Name = "Тетяна Луценко", Biography = "Вчителька, авторка підручників з математики." };
            var author15 = new Author { Id = Guid.NewGuid(), Name = "Сергій Петров", Biography = "Автор дитячих казок та оповідань." };
            var author16 = new Author { Id = Guid.NewGuid(), Name = "Анна Сергієнко", Biography = "Поетеса, авторка збірок віршів." };

            modelBuilder.Entity<Author>().HasData(author12, author13, author14, author15, author16);

            var book12 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 12",
                AuthorId = author12.Id,
                PublisherId = publisher9.Id,
                CategoryId = category15.Id,
                Price = 349.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2024, 1, 1),
                Description = "Опис Книги 12",
                Cover = CoverType.HARDCOVER,
                IsAvaliable = true
            };

            var book13 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 13",
                AuthorId = author13.Id,
                PublisherId = publisher10.Id,
                CategoryId = category16.Id,
                Price = 299.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2023, 1, 1),
                Description = "Опис Книги 13",
                Cover = CoverType.SOFT_COVER,
                IsAvaliable = true
            };

            var book14 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 14",
                AuthorId = author14.Id,
                PublisherId = publisher9.Id,
                CategoryId = category17.Id,
                Price = 249.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2022, 1, 1),
                Description = "Опис Книги 14",
                Cover = CoverType.HARDCOVER,
                IsAvaliable = true
            };

            var book15 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 15",
                AuthorId = author15.Id,
                PublisherId = publisher10.Id,
                CategoryId = category18.Id,
                Price = 199.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2021, 1, 1),
                Description = "Опис Книги 15",
                Cover = CoverType.SOFT_COVER,
                IsAvaliable = false
            };

            var book16 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 16",
                AuthorId = author16.Id,
                PublisherId = publisher9.Id,
                CategoryId = category19.Id,
                Price = 149.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2020, 1, 1),
                Description = "Опис Книги 16",
                Cover = CoverType.HARDCOVER,
                IsAvaliable = true
            };

            modelBuilder.Entity<Book>().HasData(book12, book13, book14, book15, book16);

            var feedback12 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Аліна",
                Comment = "Надзвичайно цікава біографія!",
                Rating = 5,
                Date = DateTime.UtcNow,
                IsPurchased = true,
                BookId = book12.Id
            };

            var feedback13 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Дмитро",
                Comment = "Гарне введення в філософію.",
                Rating = 4,
                Date = DateTime.UtcNow,
                IsPurchased = true,
                BookId = book13.Id
            };

            var feedback14 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Марина",
                Comment = "Чудовий підручник для школярів.",
                Rating = 5,
                Date = DateTime.UtcNow,
                IsPurchased = true,
                BookId = book14.Id
            };

            var feedback15 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Ігор",
                Comment = "Дитяча книга, що сподобалась моїм малюкам.",
                Rating = 4,
                Date = DateTime.UtcNow,
                IsPurchased = false,
                BookId = book15.Id
            };

            var feedback16 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Ірина",
                Comment = "Прекрасна поезія.",
                Rating = 5,
                Date = DateTime.UtcNow,
                IsPurchased = true,
                BookId = book16.Id
            };

            modelBuilder.Entity<Feedback>().HasData(feedback12, feedback13, feedback14, feedback15, feedback16);

            var category20 = new Category { Id = Guid.NewGuid(), Name = "Історія" };
            var category21 = new Category { Id = Guid.NewGuid(), Name = "Математика" };
            var category22 = new Category { Id = Guid.NewGuid(), Name = "Мистецтво" };
            var category23 = new Category { Id = Guid.NewGuid(), Name = "Технології" };

            modelBuilder.Entity<Category>().HasData(category20, category21, category22, category23);

            var publisher11 = new Publisher { Id = Guid.NewGuid(), Name = "Видавництво Л", Description = "Опис видавництва Л" };
            var publisher12 = new Publisher { Id = Guid.NewGuid(), Name = "Видавництво М", Description = "Опис видавництва М" };

            modelBuilder.Entity<Publisher>().HasData(publisher11, publisher12);

            var author17 = new Author { Id = Guid.NewGuid(), Name = "Олександр Мельник", Biography = "Історик, автор книг про древні цивілізації." };
            var author18 = new Author { Id = Guid.NewGuid(), Name = "Людмила Коваль", Biography = "Професор математики, автор підручників з алгебри." };
            var author19 = new Author { Id = Guid.NewGuid(), Name = "Микола Іваненко", Biography = "Митець, автор книг про живопис та музику." };
            var author20 = new Author { Id = Guid.NewGuid(), Name = "Тимур Логінов", Biography = "Автор науково-популярних книг про технології майбутнього." };

            modelBuilder.Entity<Author>().HasData(author17, author18, author19, author20);

            var book17 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 17",
                AuthorId = author17.Id,
                PublisherId = publisher11.Id,
                CategoryId = category20.Id,
                Price = 399.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2025, 1, 1),
                Description = "Опис Книги 17",
                Cover = CoverType.HARDCOVER,
                IsAvaliable = true
            };

            var book18 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 18",
                AuthorId = author18.Id,
                PublisherId = publisher12.Id,
                CategoryId = category21.Id,
                Price = 249.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2024, 1, 1),
                Description = "Опис Книги 18",
                Cover = CoverType.SOFT_COVER,
                IsAvaliable = true
            };



            var book19 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 19",
                AuthorId = author19.Id,
                PublisherId = publisher11.Id,
                CategoryId = category22.Id,
                Price = 449.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2023, 1, 1),
                Description = "Опис Книги 19",
                Cover = CoverType.HARDCOVER,
                IsAvaliable = true
            };

            var book20 = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Книга 20",
                AuthorId = author20.Id,
                PublisherId = publisher12.Id,
                CategoryId = category23.Id,
                Price = 299.99f,
                Language = Language.UKRAINIAN,
                Year = new DateTime(2022, 1, 1),
                Description = "Опис Книги 20",
                Cover = CoverType.SOFT_COVER,
                IsAvaliable = false
            };

            modelBuilder.Entity<Book>().HasData(book17, book18, book19, book20);

            var feedback17 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Ігор",
                Comment = "Чудова книга про історію цивілізацій!",
                Rating = 5,
                Date = DateTime.UtcNow,
                IsPurchased = true,
                BookId = book17.Id
            };

            var feedback18 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Світлана",
                Comment = "Чудова книга з математики для старшокласників.",
                Rating = 4,
                Date = DateTime.UtcNow,
                IsPurchased = true,
                BookId = book18.Id
            };

            var feedback19 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Олексій",
                Comment = "Ідеальна книга для шанувальників мистецтва.",
                Rating = 5,
                Date = DateTime.UtcNow,
                IsPurchased = true,
                BookId = book19.Id
            };

            var feedback20 = new Feedback
            {
                Id = Guid.NewGuid(),
                ReviewerName = "Марина",
                Comment = "Дуже цікава книга про технології майбутнього.",
                Rating = 4,
                Date = DateTime.UtcNow,
                IsPurchased = false,
                BookId = book20.Id
            };

            modelBuilder.Entity<Feedback>().HasData(feedback17, feedback18, feedback19, feedback20);


        }
    }
}