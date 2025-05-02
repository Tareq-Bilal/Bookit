using AutoMapper;
using RepositoryPatternwithUOW.Api.DTO_s.Author;
using RepositoryPatternWithUOW.EF;

namespace RepositoryPatternwithUOW.Api.Mappings.Authors
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile() {

            CreateMap<Author, AuthorAddDTO>().ReverseMap();

            CreateMap<Author, AuthorUpdateDTO>().ReverseMap();

        }
    }
}
