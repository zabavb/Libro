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
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                ;

            CreateMap<PublisherDto, Publisher>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}