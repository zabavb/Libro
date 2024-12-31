using AutoMapper;
using BookApi.Models;
using Library.DTOs.Book;

namespace BookApi.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Biography));

            CreateMap<AuthorDto, Author>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); 
        }
    }
}
