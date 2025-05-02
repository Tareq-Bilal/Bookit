using AutoMapper;
using RepositoryPatternwithUOW.Api.DTO_s.Loan;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternwithUOW.Api.Mappings
{
    public class LoanProfile : Profile
    {
        public LoanProfile() {

            CreateMap<Loan, LoanGetDTO>()

                .ForMember(dest => dest.BookTitle,
                opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : string.Empty))
                
                .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.User != null ? src.User.Name : string.Empty))
                .ReverseMap();


        }
    }
}
