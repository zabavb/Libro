using AutoMapper;

namespace BookAPI.Profiles
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<BookAPI.Models.Discount, Library.DTOs.Book.Discount>();
            CreateMap<Library.DTOs.Book.Discount, BookAPI.Models.Discount>();
        }
    }
}
