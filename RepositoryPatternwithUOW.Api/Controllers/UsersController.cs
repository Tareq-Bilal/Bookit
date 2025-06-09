using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternwithUOW.Api.DTO_s.Author;
using RepositoryPatternwithUOW.Api.DTO_s.Publisher;
using RepositoryPatternwithUOW.Api.DTO_s.User;
using RepositoryPatternwithUOW.Api.Validators.User;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Constants;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.EF;
using System.Linq.Expressions;

namespace RepositoryPatternwithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UsersController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<UserAddDTO> _userAddValidator;
        private readonly IValidator<UserUpdateDTO> _userUpdateValidator;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper
            , IValidator<UserAddDTO> userAddValidator, IValidator<UserUpdateDTO> userUpdateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userAddValidator = userAddValidator;
            _userUpdateValidator = userUpdateValidator;
        }

        [HttpGet("All")]
        [ProducesResponseType(200)] // Status code for OK
        [ProducesResponseType(404)] // Status code for OK
        public IActionResult GetAllAuthors()
        {
            var response = _unitOfWork.Users.GetAll();

            if (response == null)
                return NotFound($"Not Found Users");

            else
                return Ok(response);

        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)] // Status code for OK
        [ProducesResponseType(404)] // Status code for OK
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _unitOfWork.Users.GetByIdAsync(id);

            if (response == null)
                return NotFound($"Not Found User With Id {id}");

            else
                return Ok(response);

        }

        [HttpGet("GetInactiveUsers")]
        [ProducesResponseType(200)] // Status code for OK
        [ProducesResponseType(404)] // Status code for OK
        public async Task<IActionResult> GetInactiveUsersAsync()
        {
            var response = await _unitOfWork.Users.GetInactiveUsersAsync();

            if (response == null)
                return NotFound($"Not Found Inactive Users");

            else
                return Ok(response);

        }

        [HttpGet("SerachByName/{name}")]
        [ProducesResponseType(200)] // Status code for OK
        [ProducesResponseType(404)] // Status code for OK
        public async Task<IActionResult> SerachByNameAsync(string name)
        {
            var response = await _unitOfWork.Users.FindAllAsync(auth => auth.Name.Trim().ToLower().Contains(name.ToLower()));

            if (!response.Any())
                return NotFound($"Not Found User With Name \"{name}\" !");

            else
                return Ok(response);

        }


        [HttpGet("GetOrderByAsc")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetWithOrderByAsc([FromQuery] string orderByField = "Id")
        {
            Expression<Func<User, object>> orderByExpression = orderByField.ToLower() switch
            {
                "id" => b => b.Id,
                "name" => b => b.Name,
                "regDate" => b => b.RegistrationDate,
                _ => b => b.Id // default
            };

            var response = await _unitOfWork.Users.FindAllWithOrderingAsync(
                orderBy: orderByExpression,
                orderByDirection : OrderBy.Ascending
            );

            if (response == null || !response.Any())
                return NotFound("Not Found Users !");

            return Ok(response);

        }

        [HttpGet("GetOrderByDesc")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetOrderByDesc([FromQuery] string orderByField = "Id")
        {
            Expression<Func<User, object>> orderByExpression = orderByField.ToLower() switch
            {
                "id" => b => b.Id,
                "name" => b => b.Name,
                "regDate" => b => b.RegistrationDate,
                _ => b => b.Id // default
            };

            var response = await _unitOfWork.Users.FindAllWithOrderingAsync(
                orderBy: orderByExpression,
                orderByDirection: OrderBy.Descending
            );

            if (response == null || !response.Any())
                return NotFound("Not Found Users !");

            return Ok(response);

        }

        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Add([FromBody] UserAddDTO userAddDTO)
        {

            var validationResult = _userAddValidator.Validate(userAddDTO);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            User user = _mapper.Map<User>(userAddDTO);
            user.RegistrationDate = DateOnly.FromDateTime(DateTime.Now);

            var response = await _unitOfWork.Users.AddAsync(user);

            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok(response);

            return BadRequest($"User Addition Field !");
        }

        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UserUpdateDTO dto)
        {
            try
            {
                // Validate incoming data
                var validationResult = _userUpdateValidator.Validate(dto);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                // Fetch user by ID
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user is null)
                    return NotFound($"No User found with ID {id}");

                // Map DTO values to existing user
                _mapper.Map(dto, user);

                // Update and save
                _unitOfWork.Users.Update(user);
                var result = await _unitOfWork.CompleteAsync();

                return result > 0
                    ? Ok(user)
                    : BadRequest("User updating failed!");
            }
            catch (Exception ex)
            {
                // Log exception (optional logging line could go here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpDelete("Delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAsync([FromQuery] int Id)
        {
            var user = await _unitOfWork.Users.FindAsync(b => b.Id == Id);

            if (user == null)
                return NotFound($"Not Found User With ID {Id} !");

            else
                await _unitOfWork.Users.DeleteAsync(user);

            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok($"User With ID {Id} Deleted Successfully");

            return BadRequest($"User Deletion Field !");

        }

        [HttpPatch("Deactivate/{id}")]
        public async Task<IActionResult> DeactivateAsync(int id)
        {
            var response = await _unitOfWork.Users.GetByIdAsync(id);

            if (response == null)
                return NotFound($"Not Found User With Id {id}");

            else
            {
                await _unitOfWork.Users.DeactivateAsync(id);
                return Ok(response);
            }

        }

        [HttpGet("Count")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CountAsync()
        {

            var response = await _unitOfWork.Users.Count();

            if (response == 0)
                return NotFound("Not Found Users !");

            return Ok($"Total Of Users : {response}");

        }

    }
}
