using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternwithUOW.Api.DTO_s.Author;
using RepositoryPatternwithUOW.Api.DTO_s.Book;
using RepositoryPatternwithUOW.Api.DTO_s.BookCopy;
using RepositoryPatternwithUOW.Api.DTO_s.Category;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternwithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class BookCopyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<BookCopyAddDTO> _BookCopyAddValidator;
        private readonly IValidator<BookCopyUpdateDTO> _BookCopyUpdateValidator;
        public BookCopyController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<BookCopyAddDTO> BookCopyAddValidator,
            IValidator<BookCopyUpdateDTO> BookCopyUpdateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _BookCopyAddValidator = BookCopyAddValidator;
            _BookCopyUpdateValidator = BookCopyUpdateValidator;
        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            var bookCopies = _unitOfWork.BookCopies.GetAll();

            if (bookCopies == null)
                return NotFound("No BokkCopies Found !");

            return Ok(bookCopies);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var copy = await _unitOfWork.BookCopies.GetByIdAsync(id);

            if (copy == null)
                return NotFound($"No Book Copy found with ID {id}");

            var copyGetDto = _mapper.Map<BookCopyGetDTO>(copy);
            copyGetDto.BookTitle = _unitOfWork.Books.GetById(copyGetDto.BookId)?.Title;

            return Ok(copyGetDto);

        }

        [HttpGet("GetByTitle/{title}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByTitleAsync(string title)
        {
            // Include the related Book entity to avoid separate queries
            var copies = await _unitOfWork.BookCopies.GetBookCopiesByTitleWithBooks(title);

            if (copies == null || !copies.Any())
                return NotFound($"No book copies found with title '{title}'");

            // Map all entities at once with proper relationship handling
            var bookCopyDtos = _mapper.Map<List<BookCopyGetDTO>>(copies);

            return Ok(bookCopyDtos);
        }

        [HttpGet("GetByStatus/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByStatusAsync(string status)
        {
            var copies = await _unitOfWork.BookCopies.FindAllAsync(c => c.Status == status);

            if (copies == null || !copies.Any())
                return NotFound($"No book copies found with status \"{status}\"");

            var bookCopyDtos = _mapper.Map<List<BookCopyGetDTO>>(copies);

            return Ok(bookCopyDtos);
        }

        [HttpGet("GetBookCopiesFromTo/{From}/{To}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetYearlyPublicationCounts(DateOnly From , DateOnly To)
        {
            if (From == null || To == null) 
                return NotFound("From & To Is Required !");    

            if (From.CompareTo(To) > 0)
                return BadRequest("From Date Should Be Eralier Than To ");

            var result = await _unitOfWork.BookCopies
                .FindAllAsync(c => 
                DateOnly.FromDateTime(c.AcquisitionDate) >= From && DateOnly.FromDateTime(c.AcquisitionDate) <= To);

            if (result == null || result.Count() == 0)
                return NotFound($"Not Found Book Copies Added From \"{From}\" To \"{To}\"");

            return Ok(result);

        }


        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] BookCopyAddDTO dto)
        {
            var validationResult = await _BookCopyAddValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var copy = _mapper.Map<BookCopy>(dto);
            await _unitOfWork.BookCopies.AddAsync(copy);
            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok($"Book Copy \"{(int)copy.Id}\" Added Successfully");

            return BadRequest($"Book Copy Addition Field !");

        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] BookCopyUpdateDTO dto)
        {

            try
            {
                // First validate the input (non-DB operation)
                var validationResult = _BookCopyUpdateValidator.Validate(dto);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var copy = await _unitOfWork.BookCopies.GetByIdAsync(id);
                if (copy == null)
                    return NotFound($"No Book Copy found with ID {id}");


                // Map and update (non-DB operations)
                _mapper.Map(dto, copy);
                _unitOfWork.BookCopies.Update(copy);

                // Save changes and FULLY await it
                var rowsAffected = await _unitOfWork.CompleteAsync();

                if (rowsAffected > 0)
                    return Ok(copy);

                return BadRequest("No Book Copy updating failed!");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var copy = await _unitOfWork.BookCopies.GetByIdAsync(id);

            if (copy == null)
                return NotFound($"No Book Copy found with ID {id}");

            else
                await _unitOfWork.BookCopies.DeleteAsync(copy);

            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok($"Book Copy \"{copy.Id}\" deleted successfully.");

            else
                return BadRequest("Book Copy deletion Failed !!");


        }
    }
}
