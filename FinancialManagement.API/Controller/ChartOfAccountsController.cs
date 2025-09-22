using FinancialManagement.Application.Common.Models;
using FinancialManagement.Application.DTOs.ChartOfAccount;
using FinancialManagement.Application.Features.ChartOfAccounts.Commands;
using FinancialManagement.Application.Features.ChartOfAccounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // Uncomment if JWT/Auth is needed
    public class ChartOfAccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChartOfAccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/ChartOfAccounts
        [HttpGet]
        public async Task<IActionResult> GetChartOfAccounts([FromQuery] GetChartOfAccountsQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Internal server error",
                    Error = ex.Message
                });
            }
        }

        // GET: api/ChartOfAccounts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChartOfAccountById(int id)
        {
            var query = new GetChartOfAccountByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (!result.Success)
                return NotFound(new { Success = false, Message = result.Message });

            return Ok(result);
        }


        // POST: api/ChartOfAccounts
        [HttpPost]
        public async Task<IActionResult> CreateChartOfAccount([FromBody] CreateChartOfAccountDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = ModelState.SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage))
                    });
                }

                var currentUser = User.FindFirst(ClaimTypes.Name)?.Value ?? "System";
                var command = new CreateChartOfAccountCommand
                {
                    AccountCode = createDto.AccountCode,
                    AccountName = createDto.AccountName,
                    AccountType = createDto.AccountType,
                    Description = createDto.Description,
                    ParentAccountId = createDto.ParentAccountId,
                    OpeningBalance = createDto.OpeningBalance,
                    CreatedBy = currentUser
                };

                var result = await _mediator.Send(command);
                if (!result.Success)
                    return BadRequest(result);

                return CreatedAtAction(nameof(GetChartOfAccountById), new { id = result.Data?.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Internal server error",
                    Error = ex.Message
                });
            }
        }

        // GET: api/ChartOfAccounts/account-types
        [HttpGet("account-types")]
        [AllowAnonymous]
        public IActionResult GetAccountTypes()
        {
            try
            {
                var accountTypes = new[]
                {
                    new { Value = "Asset", Text = "Asset" },
                    new { Value = "Liability", Text = "Liability" },
                    new { Value = "Equity", Text = "Equity" },
                    new { Value = "Revenue", Text = "Revenue" },
                    new { Value = "Expense", Text = "Expense" }
                };
                return Ok(new { Success = true, Data = accountTypes });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Internal server error",
                    Error = ex.Message
                });
            }
        }
    }
}
