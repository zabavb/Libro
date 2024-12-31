using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.Book
{
    public class Feedback
    {
        public Guid FeedbackId { get; set; }
        public string ReviewerName { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public bool IsPurchased { get; set; }
    }

}
