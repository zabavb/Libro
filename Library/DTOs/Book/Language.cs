using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
