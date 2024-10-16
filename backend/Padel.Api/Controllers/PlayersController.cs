using Microsoft.AspNetCore.Mvc;
using Padel.Api.Mapping;
using Padel.Application.Repositories;
using Padel.Application.Services;
using Padel.Contracts.Requests.Player;

namespace Padel.Api.Controllers
{

    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }


        [HttpPost(ApiEndpoints.Players.Create)]
        public async Task<IActionResult> Create([FromBody] PlayerCreateRequest request, CancellationToken token)
        {
            var player = request.MapToPlayer();
            var result = await _playerService.CreateAsync(player, token);
            return CreatedAtAction(nameof(Get), new { id = player.Id }, player.MapToResponse());


        }

        [HttpGet(ApiEndpoints.Players.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
        {
            var player = await _playerService.GetByIdAsync(id, token);

            if (player is null) return NotFound();
            var response = player.MapToResponse();
            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Players.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var players = await _playerService.GetAllAsync(token);
            var playersResponse = players.MapToResponse();
            return Ok(playersResponse);
        }

        [HttpPut(ApiEndpoints.Players.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] PlayerUpdateRequest request, CancellationToken token)
        {
            var player = request.MapToPlayer(id);
            var updatedPlayer = await _playerService.UpdateAsync(player, token);

            if (updatedPlayer is null) return NotFound();
            return Ok(player.MapToResponse());

        }

        [HttpDelete(ApiEndpoints.Players.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var deleted = await _playerService.DeleteByIdAsync(id, token);

            if (!deleted) return NotFound();

            return Ok();

        }

        [HttpPost(ApiEndpoints.Players.Seed)]
        public async Task<IActionResult> SeedPlayersForSeason([FromRoute] Guid seasonId, CancellationToken token)
        {

            var result = await _playerService.SeedPlayersAsync(seasonId, amountOfPlayers: 10, token);

            if (!result) return NotFound();

            return Ok();
        }


    }
}
