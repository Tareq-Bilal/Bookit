using AutoMapper;
using AutoMapper.Configuration.Annotations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternwithUOW.Api.DTO_s.Book;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Constants;
using RepositoryPatternWithUOW.Core.DTO_s.Book;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Repositories;
using RepositoryPatternWithUOW.EF;
using System.Linq.Expressions;

namespace RepositoryPatternwithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<BookAddDTO> _addValidator;
        private readonly IValidator<BookUpdateDTO> _updateValidator;
        public BooksController(IUnitOfWork unitOfWork , IMapper mapper , IValidator<BookAddDTO> addValidator 
            , IValidator<BookUpdateDTO> updateValidator)
        {
            _unitOfWork      = unitOfWork;
            _mapper          = mapper;
            _addValidator    = addValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet("All")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(404)] 
        public IActionResult GetAll()
        {
            var response = _unitOfWork.Books.GetAll();

            if (response == null)
                return NotFound($"Books List Is Empty !");

            else
                return Ok(response);

        }

        [HttpGet("AllWithAuthor")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllWithAuthor()
        {

            var response = await _unitOfWork.Books.FindAllAsync(new[] {"Author" , "Category" });

            if (response == null || !response.Any())
                return NotFound("Not Found Books!");

            var BooksGetDTO = _mapper.Map<List<BookGetDTO>>(response);

            return Ok(BooksGetDTO);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(404)] 
        public IActionResult GetById(int id)
        {
            var response = _unitOfWork.Books.GetById(id);

            if (response == null)
                return NotFound($"Not Found Book With Id {id}");

            else
                return Ok(response);

        }

        [HttpGet("GetByTitle/{name}")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(404)] 
        public async Task<IActionResult> GetByTitle(string name)
        {
            var response = await _unitOfWork.Books.FindAsync(b => b.Title == name , new[] { "Author", "Category" });

            if (response == null)
                return NotFound($"Not Found Book With Name \"{name}\"");

            else
            {
                var BooksGetDTO = _mapper.Map<BookGetDTO>(response);
                return Ok(BooksGetDTO);

            }

        }


        [HttpGet("GetBooksByAuthor/{Id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetBooksByAuthor(int Id)
        {
            var response = await _unitOfWork.Books.GetBooksByAuthor(Id);

            if (response == null)
                return NotFound($"Not Found Book With ID \"{Id}\"");

            else
                return Ok(response);

        }

        [HttpGet("GetWithOrderByAsc")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(404)] 
        public async Task<IActionResult> GetWithOrderByAsc([FromQuery] string orderByField = "Id")
        {
            Expression<Func<Book, object>> orderByExpression = orderByField.ToLower() switch
            {
                "id" => b => b.Id,
                "name" => b => b.Title,
                "author" => b => b.Author.Name,
                _ => b => b.Id // default
            };

            var response = await _unitOfWork.Books.FindAllWithOrderingAsync(
                includes: new[] { "Author" },
                orderBy: orderByExpression,
                orderByDirection: OrderBy.Ascending
            );

            if (response == null || !response.Any())
                return NotFound("Not Found Books!");

            return Ok(response);

        }

        [HttpGet("GetWithOrderByDesc/{orderByField}")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(404)] 
        public async Task<IActionResult> GetWithOrderByDesc([FromQuery] string orderByField = "Id")
        {
            Expression<Func<Book, object>> orderByExpression = orderByField.ToLower() switch
            {
                "id" => b => b.Id,
                "name" => b => b.Title,
                "author" => b => b.Author.Name,
                _ => b => b.Id // default
            };

            var response = await _unitOfWork.Books.FindAllWithOrderingAsync(
                includes: new[] { "Author" },
                orderBy: orderByExpression,
                orderByDirection: OrderBy.Descending
            );

            if (response == null || !response.Any())
                return NotFound("Not Found Books!");

            return Ok(response);

        }
        [HttpGet("IsBookTitleExist/{Title}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> IsBookTitleExist(string Title)
        {
            if (string.IsNullOrEmpty(Title))
                return BadRequest("Please Enter The Book Title !");

            bool response = await _unitOfWork.Books.IsBookTitleExist(Title);

            if(!response)
                return NotFound("Not Found Books!");

            return Ok($"The Book \"{Title}\" is Exist");

        }

        [HttpPost("Add")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(404)] 
        public async Task<IActionResult> Add([FromBody] BookAddDTO bookAddDTO)
        {

            var validationResult = _addValidator.Validate(bookAddDTO);

            if(!validationResult.IsValid)
                return BadRequest(validationResult.Errors);


            Book book = _mapper.Map<Book>(bookAddDTO);

            var response = await _unitOfWork.Books.AddAsync(book);
            
            var RowsAffected =  _unitOfWork.Complete();

            if(RowsAffected > 0)
                return Ok(response);

            return BadRequest($"Book Addition Field !");
        }

        [HttpPut("Update")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(404)] 
        public async Task<IActionResult> UpdateAsync(int Id , [FromBody] BookUpdateDTO bookupdateDTO)
        {
            var book = await _unitOfWork.Books.FindAsync(b => b.Id == Id);

            if (book == null)
                return NotFound($"Not Found Book With ID {Id} !");

            var vaildationResult = _updateValidator.Validate(bookupdateDTO);

            if(!vaildationResult.IsValid)
                return BadRequest(vaildationResult.Errors);

            _mapper.Map(bookupdateDTO , book);

            var response = await _unitOfWork.Books.UpdateAsync(book);

            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok(response);

            return BadRequest($"Book Update Field !");

        }

        [HttpDelete("Delete")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(404)] 
        public async Task<IActionResult> DeleteAsync([FromQuery] int Id)
        {
            var book = _unitOfWork.Books.FindAsync(b => b.Id == Id);

            if (book == null)
                return NotFound($"Not Found Book With ID {Id} !");

            else
                await _unitOfWork.Books.DeleteAsync(await book);

            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok($"Book With ID {Id} Deleted Successfully");

            return BadRequest($"Book Deletion Field !");

        }


        [HttpGet("Count")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(404)] 
        public async Task<IActionResult> CountAsync()
        {

            var response = await _unitOfWork.Books.Count();

            if (response == 0)
                return NotFound("Not Found Books !");

            return Ok($"Total Of Books : {response}");

        }

        [HttpGet("CountByAuthor")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(400)]
        [ProducesResponseType(404)] 
        public async Task<IActionResult> CountByAuthor([FromQuery] string AuthorName)
        {
            if (string.IsNullOrEmpty(AuthorName))
                return BadRequest("Author Name is Empty !");

            var response = await _unitOfWork.Books.Count(b => b.Author.Name == AuthorName);

            if (response == 0)
                return NotFound($"Not Found Books Written By The Author \"{AuthorName}\" !");

            return Ok($"Total Of Books Written By The Author {AuthorName} is : {response}");

        }


    }
}
