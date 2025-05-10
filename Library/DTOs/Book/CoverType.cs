using System.ComponentModel.DataAnnotations;

namespace Library.DTOs.Book
{
    public enum CoverType
    {
        [Display(Name = "М'яка")]
        SOFTCOVER,

        [Display(Name = "Тверда")]
        HARDCOVER,

        [Display(Name = "На спіралі")]
        RINGBINDING,

        [Display(Name = "Шкіряна")]
        LEATHER,

        [Display(Name = "Суперобкладинка")]
        DUSTJACKET,
        
        [Display(Name = "Інші")]
        OTHER
    }
}
