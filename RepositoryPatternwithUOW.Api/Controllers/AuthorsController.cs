using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternwithUOW.Api.DTO_s.Author;
using RepositoryPatternwithUOW.Api.DTO_s.Book;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.DTO_s.Book;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Repositories;
using RepositoryPatternWithUOW.EF;

namespace RepositoryPatternwithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<AuthorAddDTO>    _authorAddValidator;
        private readonly IValidator<AuthorUpdateDTO> _authorUpdateValidator;

        public AuthorsController(IUnitOfWork unitOfWork , IMapper mapper 
            , IValidator<AuthorAddDTO> authorAddValidator , IValidator<AuthorUpdateDTO> authorUpdateValidator)
        {
            _unitOfWork            = unitOfWork;
            _mapper                = mapper;
            _authorAddValidator    = authorAddValidator;
            _authorUpdateValidator = authorUpdateValidator;
        }

        [HttpGet("All/")]
        [ProducesResponseType(200)] // Status code for OK
        [ProducesResponseType(404)] // Status code for OK
        public IActionResult GetAllAuthors()
        {
            var response = _unitOfWork.Authors.GetAll();

            if (response == null)
                return NotFound($"Not Found Authors");

            else
                return Ok(response);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)] // Status code for OK
        [ProducesResponseType(404)] // Status code for OK
        public IActionResult GetById(int id)
        {
            var response = _unitOfWork.Authors.GetById(id);

            if (response == null)
                return NotFound($"Not Found Author With Id {id}");
            
            else
                return Ok(response);

        }

        [HttpGet("GetByIdAsync/{id}")]
        [ProducesResponseType(200)] // Status code for OK
        [ProducesResponseType(404)] // Status code for OK
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _unitOfWork.Authors.GetByIdAsync(id);

            if (response == null)
                return NotFound($"Not Found Author With Id {id}");

            else
                return Ok(response);

        }

        [HttpGet("SerachByName/{name}")]
        [ProducesResponseType(200)] // Status code for OK
        [ProducesResponseType(404)] // Status code for OK
        public async Task<IActionResult> SerachByNameAsync(string name)
        {
            var response = await _unitOfWork.Authors
                .FindAllAsync(auth => auth.Name.Trim().ToLower().Contains(name.ToLower()));

            if (!response.Any())
                return NotFound($"Not Found Author With Name \"{name}\" !");

            else
                return Ok(response);

        }


        [HttpPost("Add")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Add([FromBody] AuthorAddDTO authorAddDTO)
        {

            var validationResult = _authorAddValidator.Validate(authorAddDTO);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            Author author = _mapper.Map<Author>(authorAddDTO);

            var response = await _unitOfWork.Authors.AddAsync(author);

            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok(response);

            return BadRequest($"Author Addition Field !");
        }

        [HttpPut("Update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateAsync(int Id, [FromBody] AuthorUpdateDTO authorUpdateDTO)
        {
            var author = await _unitOfWork.Authors.FindAsync(b => b.Id == Id);

            if (author == null)
                return NotFound($"Not Found Author With ID {Id} !");

            var vaildationResult = _authorUpdateValidator.Validate(authorUpdateDTO);

            if (!vaildationResult.IsValid)
                return BadRequest(vaildationResult.Errors);

            //Author author = _mapper.Map<Author>(authorUpdateDTO);
            _mapper.Map(authorUpdateDTO , author);
            var response  = await _unitOfWork.Authors.UpdateAsync(author);

            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok(response);

            return BadRequest($"Author Update Field !");

        }

        [HttpDelete("Delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAsync([FromQuery] int Id)
        {
            var author = await _unitOfWork.Authors.FindAsync(b => b.Id == Id);

            if (author == null)
                return NotFound($"Not Found author With ID {Id} !");

            else
                await _unitOfWork.Authors.DeleteAsync(author);

            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok($"Author With ID {Id} Deleted Successfully");

            return BadRequest($"Author Deletion Field !");

        }


        [HttpGet("Count")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CountAsync()
        {

            var response = await _unitOfWork.Authors.Count();

            if (response == 0)
                return NotFound("Not Found Authors !");

            return Ok($"Total Of Authors : {response}");

        }

        [HttpGet("CountAuthorBooks")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CountAuthorBooks([FromQuery] string AuthorName)
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
