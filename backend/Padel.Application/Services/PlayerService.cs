using Padel.Application.Models;
using Padel.Application.Repositories.Interfaces;
using Padel.Application.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Padel.Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<Player> AddPlayerAsync(Player player, CancellationToken token)
        {
            // Convert Player model to PlayerEntity
            var playerEntity = Player.ToEntity(player);
            // Add to repository and return the created entity
            var createdEntity = await _playerRepository.AddAsync(playerEntity, token);
            return Player.FromEntity(createdEntity); // Assuming AddAsync returns PlayerEntity
        }

        public async Task<bool> DeletePlayerAsync(int id, CancellationToken token)
        {
            // Call the repository to delete the player by ID
            return await _playerRepository.DeleteAsync(id, token);
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync(CancellationToken token)
        {
            // Get all players from the repository
            var playerEntities = await _playerRepository.GetAllAsync(token);
            // Convert each PlayerEntity to Player model
            var players = playerEntities.Select(playerEntity => Player.FromEntity(playerEntity));
            return players;
        }

        public async Task<Player?> GetPlayerByIdAsync(int id, CancellationToken token)
        {
            // Get the player entity from the repository
            var playerEntity = await _playerRepository.GetByIdAsync(id, token);
            // Convert to Player model if found
            return playerEntity != null ? Player.FromEntity(playerEntity) : null;
        }

        public async Task<Player> UpdatePlayerAsync(Player player, CancellationToken token)
        {
            // Convert Player model to PlayerEntity
            var playerEntity = Player.ToEntity(player);
            // Update the player in the repository
            var updatedEntity = await _playerRepository.UpdateAsync(playerEntity, token);
            // Return the updated Player model
            return Player.FromEntity(updatedEntity); // Assuming UpdateAsync returns PlayerEntity
        }
    }
}
