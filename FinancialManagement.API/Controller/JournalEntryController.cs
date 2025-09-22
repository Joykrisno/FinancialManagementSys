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
        public async Task<IActionResult> GetJournalEntries()
        {
            var result = await _mediator.Send(new GetJournalEntryListQuery());
            return Ok(result);
        }

        // ✅ Get single Journal Entry
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJournalEntry(int id)
        {
            var query = new GetJournalEntryByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // ✅ Create new Journal Entry
        [HttpPost]
        public async Task<IActionResult> CreateJournalEntry([FromBody] CreateJournalEntryCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetJournalEntry), new { id = result.Id }, result);
        }

        // ✅ Update Journal Entry
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJournalEntry(int id, [FromBody] UpdateJournalEntryCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // ✅ Delete Journal Entry
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJournalEntry(int id)
        {
            var command = new DeleteJournalEntryCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
