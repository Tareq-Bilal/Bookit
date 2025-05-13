using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternwithUOW.Api.DTO_s.Wishlist;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Models;
using System.Reflection.Metadata.Ecma335;

namespace RepositoryPatternwithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<WishlistAddDTO> _addValidator;
        private readonly IValidator<WishlistUpdateDTO> _updateValidator;

        public WishlistController
            (IUnitOfWork unitOfWork
            , IMapper mapper 
            , IValidator<WishlistAddDTO> addValidator
            , IValidator<WishlistUpdateDTO> updateValidator) { 
            
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<WishlistItemDTO>>> GetUserWishlist(int userId)
        {
            try
            {
                var wishlist = await _unitOfWork.Wishlists
                    .FindAllAsync(wl => wl.UserId == userId , new[] { "User", "Book.Author" });

                if (wishlist == null || wishlist.Count() == 0)
                    return NotFound($"Wishlist With For User {userId} is Empty !");

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [HttpPost("AddWishlistItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WishlistItemDTO>> AddToWishlist(WishlistAddDTO addDTO)
        {
            try
            {
                var validationResult = _addValidator.Validate(addDTO);
                if(!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var wishlist = _mapper.Map<Wishlist>(addDTO);
                //var itemDto  = _mapper.Map<WishlistItemDTO>(wishlist);

                await _unitOfWork.Wishlists.AddAsync(wishlist);
                await _unitOfWork.CompleteAsync();

                var updatedWishlistWithDetails = await _unitOfWork.Wishlists.GetWishlistWithDetailsAsync(wishlist.Id);

                // Map to DTO for response
                var itemDto = _mapper.Map<WishlistItemDTO>(updatedWishlistWithDetails);
                return Ok(itemDto);

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        ///// <summary>
        ///// Update an existing wishlist item
        ///// </summary>
            [HttpPut("UpdateWishlistItem/{itemId}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<WishlistItemDTO>> UpdateWishlistItem(int itemId , WishlistUpdateDTO updateDTO)
            {
                try
                {

                    var existingWishlist = await _unitOfWork.Wishlists.FindAsync(w => w.Id == itemId);
                    if (existingWishlist == null)
                        return NotFound($"Wishlist Item {itemId} Not Found!");

                    var validationResult = _updateValidator.Validate(updateDTO);
                        if (!validationResult.IsValid)
                            return BadRequest(validationResult.Errors);
                    
                    _mapper.Map(updateDTO, existingWishlist);

                    // Save changes
                    _unitOfWork.Wishlists.Update(existingWishlist);
                    await _unitOfWork.CompleteAsync(); // Changed to async version

                    var updatedWishlistWithDetails = await _unitOfWork.Wishlists.GetWishlistWithDetailsAsync(itemId);

                // Map to DTO for response
                    var itemDto = _mapper.Map<WishlistItemDTO>(updatedWishlistWithDetails);
                    return Ok(itemDto);

            }

                catch (KeyNotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(ex.Message);
                }
            }

        ///// <summary>
        ///// Remove a book from the wishlist
        ///// </summary>
        [HttpDelete("DeleteeWishlistItem/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveFromWishlist(int id)
        {
            var wihslist = await _unitOfWork.Wishlists.GetByIdAsync(id);
            if (wihslist == null)
                return NotFound($"Not Found Wishlist Item With Id {id}");

            await _unitOfWork.Wishlists.DeleteAsync(wihslist);
            await _unitOfWork.CompleteAsync();

            return Ok($"Wihslist Item With ID {id} Deleted Successfully");

        }

        ///// <summary>
        ///// Remove The User's Wishlist
        ///// </summary>
        [HttpDelete("DeleteUserWishlist/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUserWishlist(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return NotFound($"Not Found User With Id {userId}");

            int deletedCount = await _unitOfWork.Wishlists.DeleteWhereAsync(w => w.UserId == userId);
            await _unitOfWork.CompleteAsync();

            return Ok($"Wishlist for user \"{user.Name}\" cleared successfully. {deletedCount} items removed.");

        }

    }
}
