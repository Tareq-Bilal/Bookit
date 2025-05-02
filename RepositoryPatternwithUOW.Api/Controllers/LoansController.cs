using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternwithUOW.Api.DTO_s.BookCopy;
using RepositoryPatternwithUOW.Api.DTO_s.Loan;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternwithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoansController(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            var loans = _unitOfWork.Loans.GetAll();
            
            if (loans == null)
                return NotFound("No BokkCopies Found !");

            return Ok(loans);
        }

        [HttpGet("GetLoansWithDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWithDetails()
        {
            try
            {
                var loans = await _unitOfWork.Loans.GetLoansInDeatils();

                if (loans == null || !loans.Any())
                    return NotFound("No Loans Found!");

                var dtoList = _mapper.Map<List<LoanGetDTO>>(loans);

                return Ok(dtoList);
            }
            catch (Exception ex)
            {
                // Log this
                return StatusCode(500, $"Internal error: {ex.Message}");
            }


        }


    }
}
