using AutoMapper;
using BookAPI.Models;

namespace BookAPI.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
             .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
             .ReverseMap()
             .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
