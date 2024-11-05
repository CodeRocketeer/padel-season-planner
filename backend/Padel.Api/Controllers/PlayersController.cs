using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Padel.Api.Auth;
using Padel.Api.Mapping;
using Padel.Application.Services.Interfaces;
using PadelContracts.Requests.Player;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Padel.Domain.Models;

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

        [HttpGet(ApiEndpoints.Players.Get)] // GET api/players/{id}
        public async Task<ActionResult<Player>> GetPlayer(int id, CancellationToken token)
        {
            var player = await _playerService.GetPlayerByIdAsync(id, token);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }

        [HttpGet(ApiEndpoints.Players.GetAll)] // GET api/players
        public async Task<ActionResult<IEnumerable<Player>>> GetAllPlayers([FromQuery] GetAllPlayersRequest request ,CancellationToken token)
        {
            var options = request.MapToOptions();
            var players = await _playerService.GetAllPlayersAsync(options, token);

            var playersResponse = players.MapToResponse();
            return Ok(playersResponse);
        }

        [Authorize]
        [HttpPost(ApiEndpoints.Players.Create)] // POST api/players
        public async Task<ActionResult<Player>> CreatePlayer(CreatePlayerRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();

            if (userId == null) return Unauthorized();

            var player = request.MapToPlayer(userId.Value);
            var createdPlayer = await _playerService.AddPlayerAsync(player, token);
            return CreatedAtAction(nameof(GetPlayer), new { id = createdPlayer.Id }, createdPlayer);
        }

        [HttpPut(ApiEndpoints.Players.Update)] // PUT api/players/{id}
        public async Task<ActionResult<Player>> UpdatePlayer(int id, Player player, CancellationToken token)
        {
            // Ensure the player's ID matches the route parameter
            if (id != player.Id)
            {
                return BadRequest("Player ID mismatch.");
            }

            var updatedPlayer = await _playerService.UpdatePlayerAsync(player, token);
            return Ok(updatedPlayer);
        }

        [HttpDelete(ApiEndpoints.Players.Delete)] // DELETE api/players/{id}
        public async Task<ActionResult> DeletePlayer(int id, CancellationToken token)
        {
            var deleted = await _playerService.DeletePlayerAsync(id, token);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
