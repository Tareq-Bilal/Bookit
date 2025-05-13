using AutoMapper;
using RepositoryPatternwithUOW.Api.DTO_s.Review;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternwithUOW.Api.Mappings
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile() { 
        
            CreateMap<Review , ReviewGetDTO>()
                .ForMember(dest => dest.UserName , opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ReverseMap();

            CreateMap<Review , ReviewAddDTO>().ReverseMap();

            CreateMap<Review , ReviewUpdateDTO>().ReverseMap();
        
        }
    }
}
