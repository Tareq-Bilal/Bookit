using AutoMapper;
using RepositoryPatternwithUOW.Api.DTO_s.Transaction;
using RepositoryPatternWithUOW.Core.Models;
using System.Transactions;
using Transaction = RepositoryPatternWithUOW.Core.Models.Transaction;

namespace RepositoryPatternwithUOW.Api.Mappings
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {

            CreateMap<Transaction, TransactionGetDTO>()

                .ForMember(dest => dest.BookTitle,
                opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : string.Empty))

                .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.User.Name))
                .ReverseMap();

            CreateMap<Transaction, TransactionAddDTO>().ReverseMap();

            CreateMap<Transaction, TransactionUpdateDTO>().ReverseMap();

        }
    }
}
