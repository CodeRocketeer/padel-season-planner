using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Padel.Api.Mapping;
using Padel.Application.Models;
using Padel.Application.Repositories;
using Padel.Contracts.Requests.Team;

namespace Padel.Api.Controllers
{

    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;

        public TeamsController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }


        [HttpPost(ApiEndpoints.Teams.Create)]
        public async Task<IActionResult> Create([FromBody]CreateTeamRequest request)
        {
            var team = request.MapToTeam();
            var result = await _teamRepository.CreateAsync(team);
            return CreatedAtAction(nameof(Get), new { id = team.Id }, team.MapToResponse());
          

        }

        [HttpGet(ApiEndpoints.Teams.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var team = await _teamRepository.GetByIdAsync(id);

            if (team is null) return NotFound();
            var response = team.MapToResponse();
            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Teams.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _teamRepository.GetAllAsync();
            var moviesResponse = teams.MapToResponse();
            return Ok(moviesResponse);
        }

        [HttpPut(ApiEndpoints.Teams.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTeamRequest request)
        {
            var team = request.MapToTeam(id);
            var updated = await _teamRepository.UpdateAsync(team);

           if (!updated) return NotFound();

            var response = team.MapToResponse();
            return Ok(response);

        }

        [HttpDelete(ApiEndpoints.Teams.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _teamRepository.DeleteByIdAsync(id);

            if (!deleted) return NotFound();

            return Ok();

        }


    }
}
