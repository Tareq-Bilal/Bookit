using AutoMapper;
using RepositoryPatternwithUOW.Api.DTO_s.Book;
using RepositoryPatternWithUOW.Core.DTO_s.Book;
using RepositoryPatternWithUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF.Mappings.Books
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<Book, BookGetDTO>()
                .ForMember(dest => dest.AuthorName, src => src.MapFrom(src => src.Author.Name))
                .ForMember(dest => dest.Category  , src => src.MapFrom(src => src.Category.Name));

            CreateMap<Book, BookAddDTO>()
                .ForMember(dest => dest.CategoryId, src => src.MapFrom(src => src.CategoryId))
                .ReverseMap();


            CreateMap<Book, BookUpdateDTO>()
                .ForMember(dest => dest.CategoryId, src => src.MapFrom(src => src.CategoryId))
                .ReverseMap();


        }

    }
}
