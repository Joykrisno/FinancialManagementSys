using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FinancialManagement.Application.Features.ChartOfAccounts.Commands;
using FinancialManagement.Application.Features.ChartOfAccounts.Queries;
using FinancialManagement.Application.DTOs.ChartOfAccount;

namespace FinancialManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ChartOfAccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChartOfAccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetChartOfAccounts([FromQuery] GetChartOfAccountsQuery query)
        {
            try
            {
                var result = await _mediator.Send(query);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Internal server error", Error = ex.Message });
            }
        }

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
                {
                    return BadRequest(result);
                }

                return CreatedAtAction(nameof(GetChartOfAccounts), new { id = result.Data?.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Internal server error", Error = ex.Message });
            }
        }

        [HttpGet("account-types")]
        [AllowAnonymous] // This can be public as it's just returning static data
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
                return StatusCode(500, new { Success = false, Message = "Internal server error", Error = ex.Message });
            }
        }
    }
}