using AutoMapper;
using Azure;
using Azure.Core;
using BCrypt.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RepositoryPatternwithUOW.Api.DTO_s.Authentication;
using RepositoryPatternwithUOW.Api.DTO_s.Author;
using RepositoryPatternwithUOW.Api.DTO_s.User;
using RepositoryPatternwithUOW.Api.Helpers;
using RepositoryPatternwithUOW.Api.Validators.Login;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Helpers;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.EF;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternwithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork ;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IValidator<RegisterDTO> _registerValidator;

        public AuthenticationController(
            IUnitOfWork unitOfWork,
            JwtService jwtService,
            IMapper mapper,
            IValidator<RegisterDTO> registerValidator)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _mapper = mapper;
            _registerValidator = registerValidator;

        }
        
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _unitOfWork.Users.FindAsync(u => u.Email == request.Email);

            // Use YOUR verification method in the condition
            if (user == null || !VerifyPasswordService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials. Please check your email and password.");
            }

            // If verification succeeds, generate the token
            var accessToken = _jwtService.GenerateToken(_jwtService , request);
            return Ok(new { AccessToken = accessToken });
        }


        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<IActionResult> Register(RegisterDTO request)
        {
         
            var validationResult =  await _registerValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                IsActive = true, // Set sensible defaults
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now), // Use UTC time for servers
                PasswordHash = PasswordHashingService.HashPassword(request.Password)
            };


            var response = await _unitOfWork.Users.AddAsync(user);
            var RowsAffected = await _unitOfWork.CompleteAsync();

            if (RowsAffected > 0)
                return Ok(response);

            return BadRequest($"User Addition Field !");
        }
    }
}
