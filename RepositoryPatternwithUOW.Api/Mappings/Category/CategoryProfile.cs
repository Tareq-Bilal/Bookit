using AutoMapper;
using RepositoryPatternwithUOW.Api.DTO_s.Book;
using RepositoryPatternwithUOW.Api.DTO_s.Category;
using RepositoryPatternWithUOW.Core.DTO_s.Book;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternwithUOW.Api.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryAddDTO>().ReverseMap();
            
            CreateMap<Category, CategoryUpdateDTO>().ReverseMap();

            CreateMap<Category, CategoryGetDTO>().ReverseMap();


        }
    }
}
