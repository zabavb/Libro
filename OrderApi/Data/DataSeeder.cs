﻿using Microsoft.EntityFrameworkCore;
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
                    UserId = Guid.NewGuid(),
                    OrderedBooks = new List<Library.DTOs.Order.OrderedBook>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 6 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 3 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 5 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 3 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 3 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 1 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 2 }
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 2 },
                        { Guid.NewGuid(), 1 },
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
                    UserId = Guid.NewGuid(),
                    Books = new Dictionary<Guid, int>
                    {
                        { Guid.NewGuid(), 1 },
                        { Guid.NewGuid(), 1 },
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
