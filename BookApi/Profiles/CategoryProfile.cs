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
             .ForMember(dest => dest.SubcategoryIds, opt => opt.MapFrom(src => src.Subcategories.Select(f => f.Id).ToList()))
             .ReverseMap()
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ForMember(dest => dest.Subcategories, opt => opt.Ignore());

        }
    }
}
