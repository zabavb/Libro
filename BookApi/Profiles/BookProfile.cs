using AutoMapper;
using BookApi.Models;

namespace BookApi.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language))
                .ForMember(dest => dest.Cover, opt => opt.MapFrom(src => src.Cover))
                .ForMember(dest => dest.FeedbackIds, opt => opt.MapFrom(src => src.Feedbacks.Select(f => f.Id).ToList()));

            CreateMap<BookDto, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore());
        }
    }
}
