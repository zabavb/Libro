using AutoMapper;
using BookApi.Models;

namespace BookApi.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
