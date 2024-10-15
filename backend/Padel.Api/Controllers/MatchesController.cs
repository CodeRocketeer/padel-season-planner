using Microsoft.AspNetCore.Mvc;
using Padel.Api.Mapping;
using Padel.Application.Repositories;
using Padel.Application.Services;
using Padel.Contracts.Requests.Match;
using Padel.Domain.Models;

namespace Padel.Api.Controllers
{

    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }


        [HttpPost(ApiEndpoints.Matches.Create)]
        public async Task<IActionResult> Create([FromBody] MatchCreateRequest request, CancellationToken token)
        {
            var match = request.MapToMatch();
            var result = await _matchService.CreateAsync(match, token);
            return CreatedAtAction(nameof(Get), new { id = match.Id }, match.MapToResponse());


        }

        [HttpGet(ApiEndpoints.Matches.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
        {
            var match = await _matchService.GetByIdAsync(id, token);

            if (match is null) return NotFound();
            var response = match.MapToResponse();
            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Matches.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] Guid? seasonId, CancellationToken token)
        {
            IEnumerable<Match> matches;

            // Check if the seasonId parameter is provided in the query
            if (seasonId.HasValue)
            {
                // Fetch matches for the given seasonId
                matches = await _matchService.GetAllBySeasonIdAsync(seasonId.Value, token);
            }
            else
            {
                // Fetch all matches
                matches = await _matchService.GetAllAsync(token);
            }

            // If no matches are found, return a NotFound result
            if (!matches.Any())
            {
                return NotFound();
            }

            // Map the matches to response objects and return them
            var matchesResponse = matches.MapToResponse();
            return Ok(matchesResponse);
        }

        [HttpPut(ApiEndpoints.Matches.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] MatchUpdateRequest request, CancellationToken token)
        {
            var match = request.MapToMatch(id);
            var updatedMatch = await _matchService.UpdateAsync(match, token);

            if (updatedMatch is null) return NotFound();
            return Ok(match.MapToResponse());

        }

        [HttpDelete(ApiEndpoints.Matches.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var deleted = await _matchService.DeleteByIdAsync(id, token);

            if (!deleted) return NotFound();

            return Ok();

        }




    }
}
