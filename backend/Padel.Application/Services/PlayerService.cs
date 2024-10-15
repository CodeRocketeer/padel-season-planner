using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Padel.Domain.Models;
using Padel.Application.Repositories;

namespace Padel.Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<bool> CreateAsync(Player player, CancellationToken token = default)
        {
            // Player validation occurs in the Player model
            return await _playerRepository.CreateAsync(player, token);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _playerRepository.DeleteByIdAsync(id, token);
        }

        public Task<IEnumerable<Player>> GetAllAsync(CancellationToken token = default)
        {
            return _playerRepository.GetAllAsync(token);
        }

        public Task<Player?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _playerRepository.GetByIdAsync(id, token);
        }

        public async Task<Player?> UpdateAsync(Player player, CancellationToken token = default)
        {
            // Player validation occurs in the Player model
            var playerExists = await _playerRepository.ExistsByIdAsync(player.Id, token);
            if (!playerExists) return null;

            await _playerRepository.UpdateAsync(player, token);
            return player;
        }
    }
}
