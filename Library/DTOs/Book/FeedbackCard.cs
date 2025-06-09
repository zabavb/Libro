using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.Book
{
    public class FeedbackCard
    {
        public Guid FeedbackId { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public DateTime Date {  get; set; }

        public UserDisplayData UserDisplayData { get; set; }
    }

    public class UserDisplayData
    {
        public string UserName { get; set; }
        public string? UserImageUrl { get; set; }
    }

    public class FeedbackAdminCard
    {
        public Guid FeedbackId { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }

        public Guid? UserId { get; set; } // Optional, used only for composer
        public string? UserName { get; set; }

        public string Title { get; set; }
        public string? BookImageUrl { get; set;}
    }
}
