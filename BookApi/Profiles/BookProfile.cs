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
             .ForMember(dest => dest.FeedbackIds, opt => opt.MapFrom(src => src.Feedbacks.Select(f => f.Id).ToList()))
             .ReverseMap()
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ForMember(dest => dest.Feedbacks, opt => opt.Ignore());

        }
    }
}
