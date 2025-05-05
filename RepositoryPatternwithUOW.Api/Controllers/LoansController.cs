using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using RepositoryPatternwithUOW.Api.DTO_s.BookCopy;
using RepositoryPatternwithUOW.Api.DTO_s.Loan;
using RepositoryPatternwithUOW.Api.Validators.BookCopy;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Constants;
using System;

namespace RepositoryPatternwithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<LoanAddDTO>    _loanAddValidator;
        private readonly IValidator<LoanUpdateDTO> _loanUpdateValidator;
        public LoansController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<LoanAddDTO> loanAddValidator,
            IValidator<LoanUpdateDTO> loanUpdateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loanAddValidator = loanAddValidator;
            _loanUpdateValidator = loanUpdateValidator;
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

        [HttpGet("GetByID/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var loan = await _unitOfWork.Loans.FindAsync(l => l.Id == id , new[] { "User" , "Book" });
           
            if (loan == null)
                return NotFound();

            var dto = _mapper.Map<LoanGetDTO>(loan);
            return Ok(dto);
        }

        [HttpGet("GetUserLoans/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserLoansAsync(int id)
        {
            var userLoans = await _unitOfWork.Loans.FindAllAsync(l => l.UserId == id, new[] { "User", "Book" });

            if (userLoans == null)
                return NotFound($"Not Found Loans For User With ID \"{id}\"");

            var dto = _mapper.Map<List<LoanGetDTO>>(userLoans);
            return Ok(dto);
        }

        // CREATE
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddAsync([FromBody] LoanAddDTO dto)
        {
            var validationResult = await _loanAddValidator.ValidateAsync(dto);
            
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var loan = _mapper.Map<Loan>(dto);
            
            //Fetch Book Copy
            BookCopy copy = await _unitOfWork.BookCopies.GetByIdAsync(loan.BookCopyId);
            
            //Update Book Copy To "Loaned"
            copy.Status = BookCopyStatus.enStatus.Loaned.ToString();

            await _unitOfWork.Loans.AddAsync(loan);
            await _unitOfWork.BookCopies.UpdateAsync(copy);
            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok($"Loan \"{(int)loan.Id}\" Added Successfully");

            return BadRequest($"Loan Addition Field !");
        }

        // UPDATE
        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLoan(int id, [FromBody] LoanUpdateDTO dto)
        {
            try
            {
                // Validate DTO
                var validationResult = _loanUpdateValidator.Validate(dto);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                // Check for loan existence
                var loan = await _unitOfWork.Loans.GetByIdAsync(id);
                if (loan == null)
                    return NotFound($"No Loan found with ID {id}");

                //Fetch Book Copy
                BookCopy copy = await _unitOfWork.BookCopies.GetByIdAsync(loan.BookCopyId);

                //Update Book Copy To "Loaned"
                if (dto.Status == LoanStatus.enStatus.Returned.ToString())
                    copy.Status = BookCopyStatus.enStatus.Available.ToString();

                // Map and update
                _mapper.Map(dto, loan);
                _unitOfWork.Loans.Update(loan);
                _unitOfWork.BookCopies.Update(copy);

                // Save changes
                var rowsAffected = await _unitOfWork.CompleteAsync();

                if (rowsAffected > 0)
                    return Ok(loan);

                return BadRequest("Loan update failed!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // DELETE
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            try
            {
                // Check if loan exists
                var loan = await _unitOfWork.Loans.GetByIdAsync(id);

                if (loan == null)
                    return NotFound($"No Loan found with ID {id}");

                if (loan.Status != LoanStatus.enStatus.Returned.ToString())
                    return BadRequest("Not Allowed To Delete Active/Overdue Loan !");
                // Delete and commit
                await _unitOfWork.Loans.DeleteAsync(loan);
                var rowsAffected = await _unitOfWork.CompleteAsync();

                if (rowsAffected > 0)
                    return Ok($"Loan with ID \"{loan.Id}\" deleted successfully.");

                return BadRequest("Loan deletion failed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpPost("return/{loanId}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> ReturnBook(int loanId, [FromBody] BookReturnDto returnDto)
        //{
        //    try
        //    {
        //        // Check if loan exists
        //        var loan = await _unitOfWork.Loans.GetByIdAsync(loanId);
        //        if (loan == null)
        //            return NotFound($"Loan with ID {loanId} not found");

        //        // Check if loan is already returned
        //        if (loan.ReturnDate.HasValue)
        //            return BadRequest("This book has already been returned");

        //        // Process the return
        //        var result = await _bookService.ProcessBookReturnAsync(loanId, returnDto);

        //        if (result.IsSuccess)
        //            return Ok(result.Data);
        //        else
        //            return BadRequest(result.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log exception
        //        return StatusCode(500, "An error occurred while processing the book return");
        //    }
        //}

        [HttpGet("GetLoansByStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLoansByStatusAsync([FromQuery]string status = "Active")
        {

            if (!LoanStatus.Statuses.Contains(status))
                return BadRequest("Please Enter Valid Status ! , {\"Active\", \"Returned\", \"Overdue\"}");

            var loans = await _unitOfWork.Loans
                .FindAllAsync( l => l.Status == status , new[] { "User", "Book" });

            if (loans == null)
                return NotFound($"Not Found Loans With Status \"{status}\"");

            var dto = _mapper.Map<List<LoanGetDTO>>(loans);
            return Ok(dto);
        }

        [HttpGet("GetLoansByBook/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLoansByBookAsync(int id)
        {

            if(await _unitOfWork.Books.GetByIdAsync(id) == null)
                return NotFound($"Not Found Book With ID {id} ");


            var loans = await _unitOfWork.Loans
                .FindAllAsync(l => l.BookId == id , new[] { "User", "Book" });

            if (loans == null || loans.Count() == 0)
                return NotFound($"Not Found Loans For Book with ID \"{id}\"");

            var dto = _mapper.Map<List<LoanGetDTO>>(loans);
            return Ok(dto);
        }

        [HttpGet("GetLoansFromTo/{From}/{To}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLoansFromToAsync(DateOnly From, DateOnly To)
        {
            if (From == null || To == null)
                return NotFound("From & To Is Required !");

            if (From.CompareTo(To) > 0)
                return BadRequest("From Date Should Be Eralier Than To ");

            var loans = await _unitOfWork.Loans
                .FindAllAsync(c =>
                DateOnly.FromDateTime(c.BorrowDate) >= From && DateOnly.FromDateTime(c.BorrowDate) <= To);

            if (loans == null || loans.Count() == 0)
                return NotFound($"Not Found Loans Added From \"{From}\" To \"{To}\"");

            var dto = _mapper.Map<List<LoanGetDTO>>(loans);
            return Ok(dto);

        }

    }
}
