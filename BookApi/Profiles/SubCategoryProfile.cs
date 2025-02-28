using AutoMapper;
using BookAPI.Models;

namespace BookAPI.Profiles
{
    public class SubCategoryProfile: Profile
    {
        public SubCategoryProfile()
        {
            CreateMap<SubCategory, SubCategoryDto>()
                .ForMember(dest => dest.SubCategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.Category, opt => opt.Ignore());
        }
    }
}
