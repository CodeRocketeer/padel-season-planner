using Microsoft.AspNetCore.Mvc;
using Padel.Api.Mapping;

using Padel.Application.Services;
using Padel.Contracts.Requests.Team;
using Padel.Domain.Models;

namespace Padel.Api.Controllers
{
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost(ApiEndpoints.Teams.Create)]
        public async Task<IActionResult> Create([FromBody] TeamCreateRequest request,
            CancellationToken token)
        {
            var team = request.MapToTeam();
            var result = await _teamService.CreateAsync(team, token);
            return CreatedAtAction(nameof(Get), new { id = team.Id }, team.MapToResponse());
        }

        [HttpGet(ApiEndpoints.Teams.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
        {
            var team = await _teamService.GetByIdAsync(id, token);
            if (team is null) return NotFound();
            return Ok(team.MapToResponse());
        }

        [HttpGet(ApiEndpoints.Teams.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] Guid? seasonId, CancellationToken token)
        {
            IEnumerable<Team> teams;

            // Check if the seasonId parameter is provided in the query
            if (seasonId.HasValue)
                teams = await _teamService.GetAllBySeasonIdAsync(seasonId.Value, token);
            else
                teams = await _teamService.GetAllAsync(token);

            // If no teams are found, return a NotFound result
            if (!teams.Any())
                return NotFound();

            // Map the teams to response objects and return them
            var teamsResponse = teams.MapToResponse();
            return Ok(teamsResponse);
        }

        [HttpPut(ApiEndpoints.Teams.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] TeamUpdateRequest request, CancellationToken token)
        {
            var team = request.MapToTeam(id);
            var updatedTeam = await _teamService.UpdateAsync(team, token);
            if (updatedTeam is null) return NotFound();
            return Ok(updatedTeam.MapToResponse());
        }

        [HttpDelete(ApiEndpoints.Teams.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var deleted = await _teamService.DeleteByIdAsync(id, token);
            if (!deleted) return NotFound();
            return Ok();
        }
    }
}
