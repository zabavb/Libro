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
                .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.ReviewerName))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.IsPurchased, opt => opt.MapFrom(src => src.IsPurchased));

            CreateMap<FeedbackDto, Feedback>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}