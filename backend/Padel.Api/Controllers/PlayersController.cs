using Microsoft.AspNetCore.Mvc;
using Padel.Api.Mapping;
using Padel.Application.Repositories;
using Padel.Contracts.Requests.Player;

namespace Padel.Api.Controllers
{

    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayersController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }


        [HttpPost(ApiEndpoints.Players.Create)]
        public async Task<IActionResult> Create([FromBody] PlayerCreateRequest request)
        {
            var player = request.MapToPlayer();
            var result = await _playerRepository.CreateAsync(player);
            return CreatedAtAction(nameof(Get), new { id = player.Id }, player.MapToResponse());


        }

        [HttpGet(ApiEndpoints.Players.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var player = await _playerRepository.GetByIdAsync(id);

            if (player is null) return NotFound();
            var response = player.MapToResponse();
            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Players.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var players = await _playerRepository.GetAllAsync();
            var moviesResponse = players.MapToResponse();
            return Ok(moviesResponse);
        }

        [HttpPut(ApiEndpoints.Players.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] PlayerUpdateRequest request)
        {
            var player = request.MapToPlayer(id);
            var updated = await _playerRepository.UpdateAsync(player);

            if (!updated) return NotFound();

            var response = player.MapToResponse();
            return Ok(response);

        }

        [HttpDelete(ApiEndpoints.Players.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _playerRepository.DeleteByIdAsync(id);

            if (!deleted) return NotFound();

            return Ok();

        }


    }
}
