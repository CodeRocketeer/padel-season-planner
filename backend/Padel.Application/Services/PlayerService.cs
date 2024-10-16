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
        private readonly ISeasonRepository _seasonRepository;

        public PlayerService(IPlayerRepository playerRepository , ISeasonRepository seasonRepository)
        {
            _playerRepository = playerRepository;
            _seasonRepository = seasonRepository;
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

        private List<Player> GeneratePlayersForSeason(Guid seasonId, int amountOfPlayers)
        {
            var players = new List<Player>();

            for (int i = 0; i < amountOfPlayers; i++)
            {
                var player = new Player
                {
                    Id = Guid.NewGuid(),
                    Name = $"Player {i + 1}",
                    UserId = Guid.NewGuid(),
                    Sex = (i % 2 == 0) ? "M" : "F",
                    SeasonId = seasonId
                };
                players.Add(player);
            }

            return players;
            

        }

        public async Task<bool> SeedPlayersAsync(Guid seasonId, int amountOfPlayers, CancellationToken token = default)
        {
            var season = await _seasonRepository.GetByIdAsync(seasonId, token);
            if (season == null)
            {
                // If the season does not exist, return false with an error message
                return false;
            }

            var players = GeneratePlayersForSeason(seasonId, amountOfPlayers);
            await _playerRepository.CreateManyAsync(players, token);
            return true;
        }
    }
}
