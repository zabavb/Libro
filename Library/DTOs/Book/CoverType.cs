using System.ComponentModel.DataAnnotations;

namespace Library.DTOs.Book
{
    public enum CoverType
    {
        [Display(Name = "М'яка")]
        SoftCover,

        [Display(Name = "Тверда")]
        Hardcover,

        [Display(Name = "На спіралі")]
        RingBinding,

        [Display(Name = "Шкіряна")]
        Leather,

        [Display(Name = "Суперобкладинка")]
        DustJacket
    }
}
