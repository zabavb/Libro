using AutoMapper;
using BookAPI.Models;

namespace BookAPI.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>()
             .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.FeedbackIds, opt => opt.MapFrom(src => src.Feedbacks.Select(f => f.Id).ToList()))
             .ForMember(dest => dest.SubcategoryIds, opt => opt.MapFrom(src => src.Subcategories.Select(f => f.Id).ToList()))
             .ReverseMap()
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ForMember(dest => dest.Feedbacks, opt => opt.Ignore())
             .ForMember(dest => dest.Subcategories, opt => opt.Ignore());
            CreateMap<Library.DTOs.Book.BookRequest, Book>()
            //.ForMember(dest => dest.ImageUrl, opt => opt.Ignore()) 
            //.ForMember(dest => dest.AudioFileUrl, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            .ForMember(dest => dest.Feedbacks, opt => opt.Ignore()) 
            .ForMember(dest => dest.Subcategories, opt => opt.Ignore());

        }
    }
}
