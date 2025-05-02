using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternwithUOW.Api.DTO_s.Book;
using RepositoryPatternwithUOW.Api.DTO_s.Category;
using RepositoryPatternwithUOW.Api.DTO_s.Publisher;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternwithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<PublisherAddDTO> _addValidator;
        private readonly IValidator<PublisherUpdateDTO> _updateValidator;


        public PublisherController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<PublisherAddDTO> addValidator,
            IValidator<PublisherUpdateDTO> updateValidator)
        {
            _unitOfWork      = unitOfWork;
            _mapper          = mapper;
            _addValidator    = addValidator;
            _updateValidator = updateValidator;
        }

        // GET: api/Category
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = _unitOfWork.Publishers.GetAll();

            if (response == null)
                return NotFound("No Publisher Found !");

            return Ok(response);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {

            var response = await _unitOfWork.Publishers.FindAsync(c => c.Id == id);

            if (response == null)
                return NotFound($"No Publisher found with ID \"{id}\"");

            else
                return Ok(response);


        }

        [HttpGet("GetByName/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            var response = await _unitOfWork.Publishers.FindAsync(c => c.Name == name);

            if (response == null)
                return NotFound($"No Publisher found with Name \"{name}\"");

            else
                return Ok(response);

        }

        [HttpGet("GetPublishersWithBooks/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPublishersWithBookCountAsync(int id)
        {
            var publisher = await _unitOfWork.Publishers.GetByIdAsync(id);
            if (publisher == null)
                return NotFound();

            var publisherBooks = await _unitOfWork.Publishers.GetPublisherWithBooksAsync(id);
            var dto = _mapper.Map<PublisherGetDTO>(publisherBooks);
            return Ok(dto);

        }

        [HttpGet("SearchPublishersByName/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchPublishersByName(string name)
        {
            var publisher = await _unitOfWork.Publishers.SearchPublishersByNameAsync(name);

            if (publisher == null)
                return NotFound($"Not Found Publisher With Name \"{name}\"");

            return Ok(publisher);

        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddAsync([FromBody] PublisherAddDTO dto)
        {
            var validationResult = await _addValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var publisher = _mapper.Map<Publisher>(dto);
            await _unitOfWork.Publishers.AddAsync(publisher);
            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok($"Publisher \"{publisher.Name}\" Added Successfully");

            return BadRequest($"Publisher  Addition Field !");

        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] PublisherUpdateDTO dto)
        {

            try
            {

                var validationResult = _updateValidator.Validate(dto);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var publisher = await _unitOfWork.Publishers.GetByIdAsync(id);

                if (publisher == null)
                    return NotFound($"No Publisher found with ID {id}");

                _mapper.Map(dto, publisher);
                _unitOfWork.Publishers.Update(publisher);

                var rowsAffected = await _unitOfWork.CompleteAsync();

                if (rowsAffected > 0)
                    return Ok(publisher);

                return BadRequest("Publisher updating failed!");
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
            var publisher = await _unitOfWork.Publishers.GetByIdAsync(id);

            if (publisher == null)
                return NotFound($"No publisher found with ID {id}");

            else
                await _unitOfWork.Publishers.DeleteAsync(publisher);

            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok($"Publisher \"{publisher.Name}\" deleted successfully.");

            else
                return BadRequest("publisher deletion Failed !!");

        }

        [HttpGet("GetYearlyPublicationCounts/{publisherId}/{startYear}/{endYear}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetYearlyPublicationCounts(int publisherId, int startYear, int endYear)
        {
            var publisher = await _unitOfWork.Publishers.GetByIdAsync(publisherId);

            if (publisher == null)
                return NotFound($"Not Found Publisher With ID \"{publisherId}\"");

            if (endYear < startYear)
                return BadRequest("Start Year Should Be Less Than End Year !");

            var result = await _unitOfWork.Publishers.GetYearlyPublicationCountsAsync(publisherId, startYear , endYear);

            string publihserName = await _unitOfWork.Publishers.GetPublisherNameByID(publisherId);

            if (result == null || result.Count == 0)
                return NotFound($"Not Found Publicatoins From Publisher \"{publihserName}\" From {startYear} To {endYear} !");

            return Ok(result);

        }
    }
}
