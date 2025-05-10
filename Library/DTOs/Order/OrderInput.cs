using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.Order
{
    public class OrderInput
    {
        public string OrderUiId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int? ExpirationDays { get; set; }
        public OrderStatus? Status { get; set; }
        public List<BookForOrder>? Books { get; set; }
        public Guid? UserId { get; set; }
    }
}
