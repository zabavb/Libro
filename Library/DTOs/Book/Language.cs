using System.ComponentModel.DataAnnotations;

namespace Library.DTOs.Book
{
    public enum Language
    {
        [Display(Name = "Англійська")]
        ENGLISH,

        [Display(Name = "Українська")]
        UKRAINIAN,

        [Display(Name = "Іспанська")]
        SPANISH,

        [Display(Name = "Французька")]
        FRENCH,

        [Display(Name = "Німецька")]
        GERMAN,

        [Display(Name = "Other")]
        OTHER
    }
}
