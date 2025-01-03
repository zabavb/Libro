using System.ComponentModel.DataAnnotations;

namespace Library.DTOs.Book
{
    public enum Language
    {
        [Display(Name = "Англійська")]
        English,

        [Display(Name = "Українська")]
        Ukrainian,

        [Display(Name = "Іспанська")]
        Spanish,

        [Display(Name = "Французька")]
        French,

        [Display(Name = "Німецька")]
        German,

        [Display(Name = "Other")]
        Other
    }
}
