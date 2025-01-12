using AutoMapper;
using BookApi.Models;


namespace BookApi.Profiles
{
    public class PublisherProfile : Profile
    {
        public PublisherProfile()
        {
            CreateMap<Publisher, PublisherDto>()
                .ForMember(dest => dest.PublisherId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}