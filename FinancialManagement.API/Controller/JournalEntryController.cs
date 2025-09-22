using FinancialManagement.Application.Features.JournalEntries.Commands;
using FinancialManagement.Application.Features.JournalEntries.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JournalEntryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JournalEntryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ✅ Get all Journal Entries
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetJournalEntryListQuery(), cancellationToken);
            return Ok(result);
        }

        // ✅ Get single Journal Entry by Id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var query = new GetJournalEntryByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);

            return result is null ? NotFound() : Ok(result);
        }

        // ✅ Create new Journal Entry
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJournalEntryCommand command, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }


        // ✅ Update Journal Entry
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateJournalEntryCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");

            var result = await _mediator.Send(command, cancellationToken);

            return result is null ? NotFound() : Ok(result);
        }

        // ✅ Delete Journal Entry
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteJournalEntryCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);

            return !result ? NotFound() : NoContent();
        }
    }
}
