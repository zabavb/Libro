using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.User
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime EndDate { get; set; }

        public Subscription()
        {
            Title = string.Empty;
            Description = string.Empty;
            EndDate = DateTime.Now.AddDays(1);
        }
    }
}
