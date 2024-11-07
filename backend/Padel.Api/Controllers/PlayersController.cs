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
using System.Net;

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
        public async Task<ActionResult<Player>> GetPlayer(Guid userId, CancellationToken token)
        {
            var player = await _playerService.GetPlayerByIdAsync(userId, token);
            if (player == null)
            {
                throw new KeyNotFoundException($"Player with ID {userId} not found.");
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
            return CreatedAtAction(nameof(GetPlayer), new { userId = createdPlayer.UserId }, createdPlayer);
        }


    }
}
