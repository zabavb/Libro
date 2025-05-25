﻿using AutoMapper;
using BookAPI.Models;

namespace BookAPI.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>()
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Id))
            .ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<AuthorRequest, Author>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
