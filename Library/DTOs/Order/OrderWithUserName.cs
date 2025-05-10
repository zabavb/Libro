using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.Order
{
    public class OrderWithUserName
    {
        public string OrderUiId { get; set; }
        public float Price { get; set; }
        public OrderStatus Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
