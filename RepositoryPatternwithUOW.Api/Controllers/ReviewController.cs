using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternwithUOW.Api.DTO_s.Review;
using RepositoryPatternwithUOW.Api.DTO_s.Wishlist;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternwithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ReviewController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<ReviewAddDTO> _addValidator;
        private readonly IValidator<ReviewUpdateDTO> _updateValidator;

        public ReviewController
            (IUnitOfWork unitOfWork
            , IMapper mapper
            , IValidator<ReviewAddDTO> addValidator
            , IValidator<ReviewUpdateDTO> updateValidator)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<ReviewGetDTO>> GetAllReviews()
        {
            try
            {
                var review = _unitOfWork.Reviews.FindAll(new[] { "User", "Book" });

                var dto = _mapper.Map<List<ReviewGetDTO>>(review);

                return Ok(dto);

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpGet("User/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ReviewGetDTO>>> GetUserReviews(int userId)
        {
            try
            {
                var review = await _unitOfWork.Reviews
                    .FindAllAsync(wl => wl.UserId == userId, new[] { "User", "Book" });

                if (review == null || review.Count() == 0)
                    return NotFound($"User {userId} Has Not Reviews !");

                var dto = _mapper.Map<List<ReviewGetDTO>>(review);

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
        [HttpGet("Book/{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewGetDTO>> GetBookReviews(int bookId)
        {
            try
            {
                var reviews = await _unitOfWork.Reviews.FindAllAsync(wi => wi.BookId == bookId , new[] { "User" , "Book" });

                if (reviews == null || reviews.Count() == 0)
                    return NotFound($"Not Found Reviews For Book {bookId}");

                else
                {
                    var dto = _mapper.Map<List<ReviewGetDTO>>(reviews);

                    return Ok(dto);
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Add a new book to a user's wishlist
        /// </summary>
        [HttpPost("AddReview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewGetDTO>> AddReview(ReviewAddDTO addDTO)
        {
            try
            {
                var validationResult = _addValidator.Validate(addDTO);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                //Check If The User Has Already Reviwed The Book Or Not
                if (await _unitOfWork.Reviews.IsBookReviewdByTheUser(addDTO.UserId, addDTO.BookId))
                    return BadRequest($"Book {addDTO.BookId} is Alreday Reviewed By The User {addDTO.UserId}");

                var review = _mapper.Map<Review>(addDTO);

                await _unitOfWork.Reviews.AddAsync(review);
                await _unitOfWork.CompleteAsync();

                var reviewWithDetails = await _unitOfWork
                    .Reviews.FindAsync(r => r.Id == review.Id , new[] { "User" , "Book"} );

                // Map to DTO for response
                var itemDto = _mapper.Map<ReviewGetDTO>(reviewWithDetails);
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
        [HttpPut("UpdateReview/{userId}/{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewGetDTO>> UpdateReview(int userId , int bookId , ReviewUpdateDTO updateDTO)
        {
            try
            {

                var existingReview = await _unitOfWork.Reviews.FindAsync(w => w.UserId == userId && w.BookId == bookId);
                if (existingReview == null)
                    return NotFound($"Review For Book {bookId} From User {userId} Not Found!");

                var validationResult = _updateValidator.Validate(updateDTO);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                _mapper.Map(updateDTO, existingReview);

                // Save changes
                _unitOfWork.Reviews.Update(existingReview);
                await _unitOfWork.CompleteAsync(); // Changed to async version

                var reviewWithDetails = await _unitOfWork
                    .Reviews.FindAsync(r => r.Id == existingReview.Id, new[] { "User", "Book" });

                // Map to DTO for response
                var itemDto = _mapper.Map<ReviewGetDTO>(reviewWithDetails);
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
        [HttpDelete("DeleteReview/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RemoveFromWishlist(int id)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review == null)
                return NotFound($"Not Found Wishlist Item With Id {id}");

            await _unitOfWork.Reviews.DeleteAsync(review);
            await _unitOfWork.CompleteAsync();

            return Ok($"Review With ID {id} Deleted Successfully");

        }

        ///// <summary>
        ///// Remove The User's Wishlist
        ///// </summary>
        [HttpDelete("DeleteUserReviews/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUserWishlist(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return NotFound($"Not Found User With Id {userId}");

            int deletedCount = await _unitOfWork.Reviews.DeleteWhereAsync(w => w.UserId == userId);
            await _unitOfWork.CompleteAsync();

            return Ok($"Wishlist for user \"{user.Name}\" cleared successfully. {deletedCount} items removed.");

        }


    }
}
