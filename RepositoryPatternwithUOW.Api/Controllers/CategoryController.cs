using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternwithUOW.Api.DTO_s.Category;
using Azure;
using RepositoryPatternwithUOW.Api.DTO_s.Book;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.AspNetCore.Authorization;

namespace RepositoryPatternwithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryAddDTO> _createValidator;
        private readonly IValidator<CategoryUpdateDTO> _updateValidator;

        public CategoryController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CategoryAddDTO> createValidator,
            IValidator<CategoryUpdateDTO> updateValidator)
        {
            _unitOfWork      = unitOfWork;
            _mapper          = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // GET: api/Category
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllAsync()
        {
            var categories = _unitOfWork.Categories.GetAll();
            
            if(categories == null)
                return NotFound("No Categories Found !");

            return Ok(categories);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
                return NotFound($"No category found with ID {id}");

            else
            {
                var categoryGetDto = _mapper.Map<CategoryGetDTO>(category);
                return Ok(categoryGetDto);
            }

        }
        [HttpGet("GetByName/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            var category = await _unitOfWork.Categories.FindAsync(c => c.Name == name);

            if (category == null)
                return NotFound($"No category found with Name {name}");

            else
            {
                var categoryGetDto = _mapper.Map<CategoryGetDTO>(category);
                return Ok(categoryGetDto);
            }

        }

        [HttpGet("GetCategoryBooks/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryBooksAsync(int categoryId)
        {
            var categoryBooks = await _unitOfWork.Books.FindAllAsync(b => b.Category.Id == categoryId 
                                                                    , new[] { "Author", "Category" });

            if (categoryBooks == null)
                return NotFound($"No Books Found In Category with ID {categoryId}");

            else
            {
                var booksGetDto = _mapper.Map<List<BookGetDTO>>(categoryBooks);
                return Ok(booksGetDto);
            }

        }
        [HttpGet("GetCategoryBooksByName/{categoryName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryBooksByNameAsync(string categoryName)
        {
            var categoryBooks = await _unitOfWork.Books.FindAllAsync(b => b.Category.Name == categoryName 
                                                                    , new[] { "Author", "Category" });

            if (categoryBooks == null)
                return NotFound($"No Books Found In Category with ID \"{categoryName}\"");

            else
            {
                var booksGetDto = _mapper.Map<List<BookGetDTO>>(categoryBooks);
                return Ok(booksGetDto);
            }

        }
        // POST: api/Category
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CategoryAddDTO dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var category = _mapper.Map<Category>(dto);
            await _unitOfWork.Categories.AddAsync(category);
            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok($"Category \"{category}\" Added Successfully");

            return BadRequest($"Category Addition Field !");

        }

        // PUT: api/Category/5
        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryUpdateDTO dto)
        {

            try
            {
                // First validate the input (non-DB operation)
                var validationResult = _updateValidator.Validate(dto);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                // Check if category exists using a simple query first
                // This is important: we're using a SEPARATE async call and FULLY awaiting it
                bool exists = await _unitOfWork.Categories.ExistsAsync(id);
                if (!exists)
                    return NotFound($"No category found with ID {id}");

                // Now that we know it exists, get the full entity
                // FULLY await this operation before proceeding
                var category = await _unitOfWork.Categories.GetByIdAsync(id);

                // Map and update (non-DB operations)
                _mapper.Map(dto, category);
                _unitOfWork.Categories.Update(category);

                // Save changes and FULLY await it
                var rowsAffected = await _unitOfWork.CompleteAsync();

                if (rowsAffected > 0)
                    return Ok(category);

                return BadRequest("Category updating failed!");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        //// DELETE: api/Category/5
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
           
            if (category == null)
                return NotFound($"No category found with ID {id}");

            else
                await _unitOfWork.Categories.DeleteAsync(category);
            
            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok($"Category \"{category.Name}\" deleted successfully.");

            else
                return BadRequest("Category deletion Failed !!");


        }

        [HttpGet("CountByCategory/{categoryID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CountByCategory([FromQuery] int categoryID)
        {
            bool exists = await _unitOfWork.Categories.ExistsAsync(categoryID);
            if (!exists)
                return NotFound($"No category found with ID {categoryID}");

            var response = await _unitOfWork.Books.Count(b => b.Category.Id== categoryID);

            if (response == 0)
                return NotFound($"Not Found Books in Categpry With ID \"{categoryID}\" !");

            string categoryName = await _unitOfWork.Categories.GetCategoryNameByID(categoryID);
            return Ok($"Total Of Books in \"{categoryName}\" Category is : {response}");

        }

        [HttpGet("CountByCategoryName/{name}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CountByCategoryName(string name)

        {
            var exists = await _unitOfWork.Categories.ExistsAsync(name);
            if (!exists)
                return NotFound($"No category found with Name \"{name}\"");

            var response = await _unitOfWork.Books.Count(b => b.Category.Name == name);

            if (response == 0)
                return NotFound($"Not Found Books in Category \"{name}\" !");

            return Ok($"Total Of Books in \"{name}\" Category is : {response}");

        }

    }
}
