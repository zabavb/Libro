using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
