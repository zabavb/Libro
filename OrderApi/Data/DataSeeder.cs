using Microsoft.EntityFrameworkCore;
using OrderApi.Models;
using OrderStatus = Library.DTOs.Order.OrderStatus;
namespace OrderApi.Data
{
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder) 
        {
            var deliveryType1 = new DeliveryType
            {
                DeliveryId = Guid.NewGuid(),
                ServiceName = "Libro"
            };
            var deliveryType2 = new DeliveryType
            {
                DeliveryId = Guid.NewGuid(),
                ServiceName = "Ukr Post"
            };
            var deliveryType3 = new DeliveryType
            {
                DeliveryId = Guid.NewGuid(),
                ServiceName = "Nova Post"
            };

            modelBuilder.Entity<DeliveryType>().HasData(deliveryType1, deliveryType2, deliveryType3);

            var order1 = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                BookIds = { Guid.NewGuid() },
                Region = "Lviv",
                City = "Lviv",
                Address = "Address1",
                Price = (float)100.59,
                DeliveryTypeId = deliveryType1.DeliveryId,
                DeliveryPrice = (float)120.59,
                OrderDate = DateTime.Now,
                DeliveryDate = DateTime.Now.AddDays(2),
                Status = OrderStatus.PROCESSING,
            };
            var order2 = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                BookIds = { Guid.NewGuid() },
                Region = "Lviv",
                City = "Lviv",
                Address = "Address2",
                Price = (float)100.59,
                DeliveryTypeId = deliveryType2.DeliveryId,
                DeliveryPrice = (float)120.59,
                OrderDate = DateTime.Now,
                DeliveryDate = DateTime.Now.AddDays(7),
                Status = OrderStatus.TRANSIT,
            };

            var order3 = new Order
            {
                OrderId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                BookIds = { Guid.NewGuid() },
                Region = "Lviv",
                City = "Lviv",
                Address = "Address3",
                Price = (float)100.59,
                DeliveryTypeId = deliveryType3.DeliveryId,
                DeliveryPrice = (float)120.59,
                OrderDate = DateTime.Now,
                DeliveryDate = DateTime.Now.AddDays(4),
                Status = OrderStatus.COMPLETED,
            };

            modelBuilder.Entity<Order>().HasData(order1, order2, order3);
        }
    }
}
