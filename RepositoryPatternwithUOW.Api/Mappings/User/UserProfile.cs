using AutoMapper;
using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.User;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternwithUOW.Api.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserAddDTO>().ReverseMap();
            
            CreateMap<User, UserGetDTO>().ReverseMap();

            CreateMap<User, UserUpdateDTO>().ReverseMap();

        }
    }
}
