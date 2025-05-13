using AutoMapper;
using RepositoryPatternwithUOW.Api.DTO_s.Wishlist;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternwithUOW.Api.Mappings
{
    public class WishlistProfile : Profile
    {
        public WishlistProfile()
        {
            // Entity to DTO mappings

            // Wishlist -> WishlistItemBasicDTO
            CreateMap<Wishlist, WishlistGetDTO>().ReverseMap(); ;

            // Wishlist -> WishlistItemDTO (with User and Book properties)
            CreateMap<Wishlist, WishlistItemDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.BookAuthor, opt => opt.MapFrom(src => src.Book.Author.Name))
                .ReverseMap();

            // DTO to Entity mappings

            // AddWishlistItemDTO -> Wishlist
            CreateMap<WishlistAddDTO, Wishlist>()
                .ForMember(dest => dest.AddedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ReverseMap();

            // UpdateWishlistItemDTO -> Wishlist
            CreateMap<WishlistUpdateDTO, Wishlist>()
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.AddedDate, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Book, opt => opt.Ignore());
        
        }
    }
}
