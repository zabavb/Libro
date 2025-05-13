using Microsoft.EntityFrameworkCore;
using OrderApi.Models;
using System.Text.Json;
using OrderStatus = Library.DTOs.Order.OrderStatus;
namespace OrderApi.Data
{
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var deliveryTypes = new List<DeliveryType> {
                new DeliveryType
                {
                    DeliveryId = Guid.NewGuid(),
                    ServiceName = "Libro"
                },
                new DeliveryType
                {
                    DeliveryId = Guid.NewGuid(),
                    ServiceName = "Ukr Post"
                },
                new DeliveryType
                {
                    DeliveryId = Guid.NewGuid(),
                    ServiceName = "Nova Post"
                },
                new DeliveryType
                {
                    DeliveryId = Guid.NewGuid(),
                    ServiceName = "Meest"
                },
            };

            modelBuilder.Entity<DeliveryType>().HasData(deliveryTypes);

            var orders = new List<Order> {
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("eb65e5c5-a3bd-4da7-9c43-e722c49c0151"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 1 },
                        { Guid.Parse("336618f9-fd33-4775-90ab-c6e895f60b0a"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Stepana Bandery St. 52",
                    Price = (float)100.59,
                    DeliveryTypeId = deliveryTypes[0].DeliveryId,
                    DeliveryPrice = (float)120.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.PROCESSING,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("eb65e5c5-a3bd-4da7-9c43-e722c49c0151"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("336618f9-fd33-4775-90ab-c6e895f60b0a"), 1 },
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 1 },
                        { Guid.Parse("05f3ed9d-05aa-4421-addf-e85906260f64"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Heroiv UPA ST. 12",
                    Price = (float)100.59,
                    DeliveryTypeId = deliveryTypes[1].DeliveryId,
                    DeliveryPrice = (float)120.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(7),
                    Status = OrderStatus.TRANSIT,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("eb65e5c5-a3bd-4da7-9c43-e722c49c0151"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("05f3ed9d-05aa-4421-addf-e85906260f64"), 1 },
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Antonovycha St. 2",
                    Price = (float)100.59,
                    DeliveryTypeId = deliveryTypes[2].DeliveryId,
                    DeliveryPrice = (float)120.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(4),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("0497d9e4-ec6b-4277-a268-18f61104e140"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("d0fa5452-d1c7-420d-b0ed-0e7feb55ec4f"), 1 },
                        { Guid.Parse("336618f9-fd33-4775-90ab-c6e895f60b0a"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Zelena St. 34",
                    Price = (float)100.59,
                    DeliveryTypeId = deliveryTypes[3].DeliveryId,
                    DeliveryPrice = (float)120.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("0497d9e4-ec6b-4277-a268-18f61104e140"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("a943fd38-91bc-42de-8316-f67ca4d292db"), 1 },
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 2 }
                    },
                    Region = "Kyiv",
                    City = "Kyiv",
                    Address = "Hetmana Pavla Scoropadskoho St. 34",
                    Price = (float)100.59,
                    DeliveryTypeId = deliveryTypes[3].DeliveryId,
                    DeliveryPrice = (float)120.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("0497d9e4-ec6b-4277-a268-18f61104e140"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 1 }
                    },
                    Region = "Kyiv",
                    City = "Kyiv",
                    Address = "Hetmana Pavla Scoropadskoho St. 12",
                    Price = (float)100.59,
                    DeliveryTypeId = deliveryTypes[2].DeliveryId,
                    DeliveryPrice = (float)120.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("56a1c91f-8b00-43fa-adac-19b67559c48d"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("7869777a-ae20-417d-8574-1b6800e0608f"), 6 },
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Stepana Bandery St. 32",
                    Price = (float)100.59,
                    DeliveryTypeId = deliveryTypes[0].DeliveryId,
                    DeliveryPrice = (float)120.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.PROCESSING,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("56a1c91f-8b00-43fa-adac-19b67559c48d"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 3 },
                        { Guid.Parse("9d80eb19-a069-4267-befe-9ffd1959312e"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Heroiv UPA ST. 12",
                    Price = (float)100.59,
                    DeliveryTypeId = deliveryTypes[1].DeliveryId,
                    DeliveryPrice = (float)120.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(7),
                    Status = OrderStatus.TRANSIT,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("47f7e164-97d2-4f0c-ae58-173b38554658"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("9d80eb19-a069-4267-befe-9ffd1959312e"), 1 },
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Antonovycha St. 59",
                    Price = (float)89.99,
                    DeliveryTypeId = deliveryTypes[2].DeliveryId,
                    DeliveryPrice = (float)120.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(4),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("47f7e164-97d2-4f0c-ae58-173b38554658"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("9d80eb19-a069-4267-befe-9ffd1959312e"), 1 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Zelena St. 68",
                    Price = (float)156.59,
                    DeliveryTypeId = deliveryTypes[3].DeliveryId,
                    DeliveryPrice = (float)120.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("da4f5a5b-f2c3-4608-bd34-bf0b6431e75c"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("048e793f-e796-40a1-b535-7fae46d0cbc6"), 5 },
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 2 }
                    },
                    Region = "Kyiv",
                    City = "Kyiv",
                    Address = "Hetmana Pavla Scoropadskoho St. 21",
                    Price = (float)100.59,
                    DeliveryTypeId = deliveryTypes[3].DeliveryId,
                    DeliveryPrice = (float)120.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("da4f5a5b-f2c3-4608-bd34-bf0b6431e75c"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("048e793f-e796-40a1-b535-7fae46d0cbc6"), 1 },
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 2 }
                    },
                    Region = "Kyiv",
                    City = "Kyiv",
                    Address = "Zhylianska St. 52",
                    Price = (float)100.59,
                    DeliveryTypeId = deliveryTypes[2].DeliveryId,
                    DeliveryPrice = (float)120.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("1d848cea-362d-40d9-8821-4bc984e5e25a"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("048e793f-e796-40a1-b535-7fae46d0cbc6"), 1 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Stepana Bandery St. 10",
                    Price = (float)92.39,
                    DeliveryTypeId = deliveryTypes[0].DeliveryId,
                    DeliveryPrice = (float)58.99,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.PROCESSING,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("1d848cea-362d-40d9-8821-4bc984e5e25a"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 1 },
                        { Guid.Parse("5f99ca9e-4115-4ed8-8fad-b2d7d9609151"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Heroiv UPA ST. 62",
                    Price = (float)200.00,
                    DeliveryTypeId = deliveryTypes[1].DeliveryId,
                    DeliveryPrice = (float)150.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(7),
                    Status = OrderStatus.TRANSIT,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("1d848cea-362d-40d9-8821-4bc984e5e25a"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("d0fa5452-d1c7-420d-b0ed-0e7feb55ec4f"), 1 },
                        { Guid.Parse("5f99ca9e-4115-4ed8-8fad-b2d7d9609151"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Antonovycha St. 50",
                    Price = (float)199.99,
                    DeliveryTypeId = deliveryTypes[2].DeliveryId,
                    DeliveryPrice = (float)10.00,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(4),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("d605c572-6210-4840-918c-5348fe63815a"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("d0fa5452-d1c7-420d-b0ed-0e7feb55ec4f"), 1 },
                        { Guid.Parse("5f99ca9e-4115-4ed8-8fad-b2d7d9609151"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Zelena St. 3",
                    Price = (float)99.99,
                    DeliveryTypeId = deliveryTypes[3].DeliveryId,
                    DeliveryPrice = (float)9.99,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("d605c572-6210-4840-918c-5348fe63815a"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("1cf76a75-54d8-478c-8d22-3d22811f880a"), 3 },
                        { Guid.Parse("9d80eb19-a069-4267-befe-9ffd1959312e"), 2 }
                    },
                    Region = "Kyiv",
                    City = "Kyiv",
                    Address = "Hetmana Pavla Scoropadskoho St. 80",
                    Price = (float)400.00,
                    DeliveryTypeId = deliveryTypes[3].DeliveryId,
                    DeliveryPrice = (float)4.99,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("34c5f3c9-9b9a-490c-91c2-1a88a22226bc"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("1cf76a75-54d8-478c-8d22-3d22811f880a"), 1 },
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 2 }
                    },
                    Region = "Kyiv",
                    City = "Kyiv",
                    Address = "Hetmana Pavla Scoropadskoho St. 5",
                    Price = (float)168.59,
                    DeliveryTypeId = deliveryTypes[2].DeliveryId,
                    DeliveryPrice = (float)3.99,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("34c5f3c9-9b9a-490c-91c2-1a88a22226bc"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 3 },
                        { Guid.Parse("1cf76a75-54d8-478c-8d22-3d22811f880a"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Stepana Bandery St. 21",
                    Price = (float)189.99,
                    DeliveryTypeId = deliveryTypes[0].DeliveryId,
                    DeliveryPrice = (float)12.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.PROCESSING,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("f3c715ba-7478-43de-a35a-08cef0d55c27"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("72832bdc-41be-41a2-a1cb-05942832e8f2"), 1 },
                        { Guid.Parse("553b2b85-cc6e-4dad-b468-8486f5be9a8f"), 1 },
                        { Guid.Parse("2a6bab6e-7324-4914-9d23-b26726762a1d"), 1 },
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 1 },
                        { Guid.Parse("5f99ca9e-4115-4ed8-8fad-b2d7d9609151"), 1 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Heroiv UPA ST. 14",
                    Price = (float)65.99,
                    DeliveryTypeId = deliveryTypes[1].DeliveryId,
                    DeliveryPrice = (float)4.99,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(7),
                    Status = OrderStatus.TRANSIT,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("f3c715ba-7478-43de-a35a-08cef0d55c27"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("df76d28a-1309-4d04-86cc-58eccd373ba3"), 1 },
                        { Guid.Parse("5f99ca9e-4115-4ed8-8fad-b2d7d9609151"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Antonovycha St. 23",
                    Price = (float)79.99,
                    DeliveryTypeId = deliveryTypes[2].DeliveryId,
                    DeliveryPrice = (float)23.99,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(4),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("b86c042d-a6f0-4cb8-90b4-31060f0af325"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("9b0a735f-593e-41fb-ba99-25d7631d792d"), 1 },
                        { Guid.Parse("64ce1687-3624-4f97-80e4-142e216f3c5d"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Zelena St. 62",
                    Price = (float)156.59,
                    DeliveryTypeId = deliveryTypes[3].DeliveryId,
                    DeliveryPrice = (float)120.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("b86c042d-a6f0-4cb8-90b4-31060f0af325"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("64ce1687-3624-4f97-80e4-142e216f3c5d"), 1 },
                        { Guid.Parse("9b0a735f-593e-41fb-ba99-25d7631d792d"), 2 }
                    },
                    Region = "Kyiv",
                    City = "Kyiv",
                    Address = "Hetmana Pavla Scoropadskoho St. 17",
                    Price = (float)109.29,
                    DeliveryTypeId = deliveryTypes[3].DeliveryId,
                    DeliveryPrice = (float)2.99,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("85ddedba-fb4a-4ba9-aa1c-ad96585269a5"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("64ce1687-3624-4f97-80e4-142e216f3c5d"), 1 },
                        { Guid.Parse("398fff8e-7803-46a3-b72e-25b9b56b8789"), 2 }
                    },
                    Region = "Kyiv",
                    City = "Kyiv",
                    Address = "Zhylianska St. 52",
                    Price = (float)29.99,
                    DeliveryTypeId = deliveryTypes[2].DeliveryId,
                    DeliveryPrice = (float)3.99,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("85ddedba-fb4a-4ba9-aa1c-ad96585269a5"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("398fff8e-7803-46a3-b72e-25b9b56b8789"), 1 },
                        { Guid.Parse("9b0a735f-593e-41fb-ba99-25d7631d792d"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Stepana Bandery St. 12",
                    Price = (float)92.39,
                    DeliveryTypeId = deliveryTypes[0].DeliveryId,
                    DeliveryPrice = (float)58.99,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.PROCESSING,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("472855a2-96a7-46b2-9492-9b25af4fab98"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("142f24c3-271a-4d70-bab4-be0612290e80"), 1 },
                        { Guid.Parse("398fff8e-7803-46a3-b72e-25b9b56b8789"), 2 }
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Heroiv UPA ST. 24",
                    Price = (float)200.00,
                    DeliveryTypeId = deliveryTypes[1].DeliveryId,
                    DeliveryPrice = (float)150.59,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(7),
                    Status = OrderStatus.TRANSIT,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("472855a2-96a7-46b2-9492-9b25af4fab98"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("9b0a735f-593e-41fb-ba99-25d7631d792d"), 1 },
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Antonovycha St. 15",
                    Price = (float)199.99,
                    DeliveryTypeId = deliveryTypes[2].DeliveryId,
                    DeliveryPrice = (float)10.00,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(4),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("6d79fbcb-18f5-4e4d-a942-617a5bb8d930"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("142f24c3-271a-4d70-bab4-be0612290e80"), 1 },
                    },
                    Region = "Lviv",
                    City = "Lviv",
                    Address = "Zelena St. 95",
                    Price = (float)99.99,
                    DeliveryTypeId = deliveryTypes[3].DeliveryId,
                    DeliveryPrice = (float)9.99,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("6d79fbcb-18f5-4e4d-a942-617a5bb8d930"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("9b0a735f-593e-41fb-ba99-25d7631d792d"), 2 },
                        { Guid.Parse("142f24c3-271a-4d70-bab4-be0612290e80"), 1 },
                    },
                    Region = "Kyiv",
                    City = "Kyiv",
                    Address = "Hetmana Pavla Scoropadskoho St. 52",
                    Price = (float)400.00,
                    DeliveryTypeId = deliveryTypes[3].DeliveryId,
                    DeliveryPrice = (float)4.99,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    UserId = Guid.Parse("6d79fbcb-18f5-4e4d-a942-617a5bb8d930"),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.Parse("398fff8e-7803-46a3-b72e-25b9b56b8789"), 1 },
                        { Guid.Parse("142f24c3-271a-4d70-bab4-be0612290e80"), 1 },
                    },
                    Region = "Kyiv",
                    City = "Kyiv",
                    Address = "Hetmana Pavla Scoropadskoho St. 9",
                    Price = (float)168.59,
                    DeliveryTypeId = deliveryTypes[2].DeliveryId,
                    DeliveryPrice = (float)3.99,
                    OrderDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(2),
                    Status = OrderStatus.COMPLETED,
                },
            };





            modelBuilder.Entity<Order>().HasData(orders);
        }
    }
}
