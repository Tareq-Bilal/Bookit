using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternwithUOW.Api.DTO_s.Wishlist;
using RepositoryPatternWithUOW.Core;

namespace RepositoryPatternwithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WishlistController
            (IUnitOfWork unitOfWork
            , IMapper mapper) { 
            
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("All")]
        public ActionResult<List<WishlistItemDTO>> GetAllWishlists()
        {
            try
            {
                var wishlist = _unitOfWork.Wishlists.FindAll(new[] {"User" , "Book.Author"});
                
                var dto = _mapper.Map<List<WishlistItemDTO>>(wishlist);
                
                return Ok(dto);

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<WishlistItemDTO>>> GetUserWishlist(int userId)
        {
            try
            {
                var wishlist = await _unitOfWork.Wishlists
                    .FindAllAsync(wl => wl.UserId == userId , new[] { "User", "Book.Author" });

                var dto = _mapper.Map<List<WishlistItemDTO>>(wishlist);

                return Ok(dto);

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get a specific wishlist item by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<WishlistItemDTO>> GetWishlistItem(int id)
        {
            try
            {
                var wishlistItem = await _unitOfWork.Wishlists.FindAsync(wi => wi.Id == id);
                return Ok(wishlistItem);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Add a new book to a user's wishlist
        /// </summary>
        //[HttpPost]
        //public async Task<ActionResult<WishlistItemDTO>> AddToWishlist(AddWishlistItemDTO addDTO)
        //{
        //    try
        //    {
        //        var createdItem = await _wishlistService.AddToWishlistAsync(addDTO);
        //        return CreatedAtAction(
        //            nameof(GetWishlistItem),
        //            new { id = createdItem.Id },
        //            createdItem);
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Update an existing wishlist item
        ///// </summary>
        //[HttpPut]
        //public async Task<ActionResult<WishlistItemDTO>> UpdateWishlistItem(UpdateWishlistItemDTO updateDTO)
        //{
        //    try
        //    {
        //        var updatedItem = await _wishlistService.UpdateWishlistItemAsync(updateDTO);
        //        return Ok(updatedItem);
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Remove a book from the wishlist
        ///// </summary>
        //[HttpDelete("{id}")]
        //public async Task<ActionResult> RemoveFromWishlist(int id)
        //{
        //    var result = await _wishlistService.RemoveFromWishlistAsync(id);
        //    if (!result)
        //    {
        //        return NotFound($"Wishlist item with ID {id} not found");
        //    }

        //    return NoContent();
        //}



    }
}
