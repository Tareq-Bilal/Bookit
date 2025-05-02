using AutoMapper;
using RepositoryPatternwithUOW.Api.DTO_s.Publisher;
using RepositoryPatternWithUOW.EF.Repositories;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternwithUOW.Api.DTO_s.Book;
using RepositoryPatternWithUOW.Core.DTO_s.Book;
using RepositoryPatternwithUOW.Api.Validators.Publisher;

namespace RepositoryPatternwithUOW.Api.Mappings
{
    public class PublisherProfile : Profile
    {
        public PublisherProfile() {

            CreateMap<Publisher, PublisherGetDTO >().ReverseMap();
            
            CreateMap<Publisher, PublisherAddDTO >().ReverseMap();
            
            CreateMap<Publisher, PublisherUpdateDTO>().ReverseMap();

        }


    }
}
