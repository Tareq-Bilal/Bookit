using AutoMapper;
using RepositoryPatternwithUOW.Api.DTO_s.BookCopy;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternwithUOW.Api.Mappings
{
    public class BookCopyProfile : Profile
    {
        
        public BookCopyProfile()
        {

            CreateMap<BookCopy, BookCopyGetDTO>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title));

            CreateMap<BookCopy, BookCopyAddDTO>().ReverseMap();

            CreateMap<BookCopy, BookCopyUpdateDTO>().ReverseMap();

        }

    }
}
