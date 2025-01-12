using AutoMapper;
using BookApi.Models;

namespace BookApi.Profiles
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedback, FeedbackDto>()
                .ForMember(dest => dest.FeedbackId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}