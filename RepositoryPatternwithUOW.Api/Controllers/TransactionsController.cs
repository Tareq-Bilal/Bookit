using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternwithUOW.Api.DTO_s.Loan;
using RepositoryPatternwithUOW.Api.DTO_s.Transaction;
using RepositoryPatternwithUOW.Api.Validators.Loan;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Constants;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternwithUOW.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class TransactionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<TransactionAddDTO> _TransactionAddValidator;
        private readonly IValidator<TransactionUpdateDTO> _TransactionUpdateValidator;

        public TransactionsController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<TransactionAddDTO> TransactionAddValidator,
            IValidator<TransactionUpdateDTO> TransactionUpdateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _TransactionAddValidator = TransactionAddValidator;
            _TransactionUpdateValidator = TransactionUpdateValidator;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TransactionGetDTO>>> GetAll()
        {
            var transactions = await _unitOfWork.Transactions.FindAllAsync(new[] {"User" , "Book"});

            if (transactions == null || transactions.Count() == 0)
                return NotFound("NOT Found Transactions");

            var dto = _mapper.Map<List<TransactionGetDTO>>(transactions);
            return Ok(dto);
        }

        /// <summary>
        /// Gets a transaction by id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransactionGetDTO>> GetById(int id)
        {
            var transaction = await _unitOfWork.Transactions.FindAsync(t => t.Id == id , new[] { "User", "Book" });

            if (transaction == null)
                return NotFound($"NOT Found Transaction With ID {id}");

            var dto = _mapper.Map<TransactionGetDTO>(transaction);
            return Ok(dto);
        }

        /// <summary>
        /// Gets transactions summary data
        /// </summary>
        [HttpGet("Summary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TransactionSummaryDTO>> GetSummary()
        {
            TransactionSummaryDTO dto = new TransactionSummaryDTO();

            dto.TotalCount   = await _unitOfWork.Transactions.Count();
            dto.TotalAmount  = await _unitOfWork.Transactions.GetTotalTransactionsAmountAsync();
            dto.AmountByType = await _unitOfWork.Transactions.AmountByTypeAsync();
            dto.CountByType  = await _unitOfWork.Transactions.CountByTypeAsync();

            return Ok(dto);

        }

        /// <summary>
        /// Gets all transactions for a specific user
        /// </summary>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TransactionGetDTO>>> GetByUser(int userId)
        {
            if (await _unitOfWork.Users.GetByIdAsync(userId) == null)
                return NotFound($"Not Found User With ID {userId}");

            var userTransactions = await _unitOfWork.Transactions.FindAllAsync(t => t.UserId == userId);

            if (userTransactions == null || userTransactions.Count() == 0)
                return NotFound($"Not Found Transactoins For The User With ID {userId}");

            var dto = _mapper.Map<List<TransactionGetDTO>>(userTransactions);
            return Ok(dto);

        }

        /// <summary>
        /// Gets all transactions for a specific loan
        /// </summary>
        [HttpGet("loan/{loanId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TransactionGetDTO>>> GetByLoan(int loanId)
        {
            if (await _unitOfWork.Loans.GetByIdAsync(loanId) == null)
                return NotFound($"Not Found Loan With ID {loanId}");

            var loanTransactions = await _unitOfWork.Transactions.FindAllAsync(t => t.LoanId == loanId);

            if (loanTransactions == null || loanTransactions.Count() == 0)
                return NotFound($"Not Found Transactoins For The Loan With ID {loanId}");

            var dto = _mapper.Map<List<TransactionGetDTO>>(loanTransactions);
            return Ok(dto);
        }

        /// <summary>
        /// Creates a new transaction
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransactionAddDTO>> Create([FromBody] TransactionAddDTO createDto)
        {

            var validationResult = await _TransactionAddValidator.ValidateAsync(createDto);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var transaction = _mapper.Map<Transaction>(createDto);

            var result = _mapper.Map<TransactionGetDTO>(await _unitOfWork.Transactions.AddAsync(transaction));

            var RowsAffected = _unitOfWork.Complete();

            if (RowsAffected > 0)
                return Ok(result);
            //return Ok($"Transaction \"{(int)result.Id}\" Added Successfully");
            return BadRequest($"Transaction Addition Field !");

        }

        /// <summary>
        /// Updates an existing transaction
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TransactionUpdateDTO>> Update(int id, [FromBody] TransactionUpdateDTO updateDto)
        {
            try
            {
                // Validate DTO
                var validationResult = _TransactionUpdateValidator.Validate(updateDto);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                // Check for loan existence
                var transaction = await _unitOfWork.Transactions.FindAsync(t => t.Id == id, new[] { "User", "Book" });
                if (transaction == null)
                    return NotFound($"No Transactoin found with ID {id}");

                // Map and update
                _mapper.Map(updateDto, transaction);
                _unitOfWork.Transactions.Update(transaction);

                // Save changes
                var rowsAffected = await _unitOfWork.CompleteAsync();

                if (rowsAffected > 0)
                {
                    var dto = _mapper.Map<TransactionGetDTO>(transaction);
                    return Ok(dto);

                }

                return BadRequest("Transactoin update failed!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a transaction
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            
            if (transaction == null)
                return NotFound($"No Transactoin found with ID {id}");

            await _unitOfWork.Transactions.DeleteAsync(transaction);
            var rowsAffected = await _unitOfWork.CompleteAsync();

            if (rowsAffected > 0)
                return Ok($"Transaction with ID \"{transaction.Id}\" deleted successfully.");

            return BadRequest("Transaction deletion failed.");

        }

        [HttpGet("GetAllByTransactionType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TransactionGetDTO>>> GetAllByTransactionType([FromQuery] string type = "Fine")
        {
            if (!TransactionType.TransactionTypes.Contains(type))
                return NotFound($"Not Found Transaction Type \"{type}\" , " +
                                    $"Choose on of valid types [Fee, Fine, Damage, Refund]");
                                                                 
            var transactions = await _unitOfWork.Transactions.FindAllAsync(t => t.TransactionType == type);

            if (transactions == null || transactions.Count() == 0)
                return NotFound($"Not Found Transactoins with type \"{type}\"");

            var dto = _mapper.Map<List<TransactionGetDTO>>(transactions);
            return Ok(dto);
        }

        [HttpGet("GetTransactionsFromTo/{From}/{To}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactionsFromTo(DateOnly From, DateOnly To)
        {
            if (From == null || To == null)
                return NotFound("From & To Is Required !");

            if (From.CompareTo(To) > 0)
                return BadRequest("From Date Should Be Eralier Than To ");

            var transactions = await _unitOfWork.Transactions
                .FindAllAsync(c =>
                DateOnly.FromDateTime(c.TransactionDate) >= From && DateOnly.FromDateTime(c.TransactionDate) <= To);

            if (transactions == null || transactions.Count() == 0)
                return NotFound($"Not Found Transactions Added From \"{From}\" To \"{To}\"");

            var dto = _mapper.Map<List<TransactionGetDTO>>(transactions);
            return Ok(dto);

        }


    }
}
