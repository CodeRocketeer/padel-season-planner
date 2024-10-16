using Padel.Domain.Models;
using Padel.Infrastructure.Entities;

namespace Padel.Application.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        // Storing PlayerEntity instances
        private readonly List<PlayerEntity> _playerEntities = new();

        public Task<bool> CreateAsync(Player player, CancellationToken token = default)
        {
            // Check if a PlayerEntity with the same Id already exists
            if (_playerEntities.Any(pe => pe.Id == player.Id))
            {
                return Task.FromResult(false); // Player already exists
            }

            // Create a PlayerEntity from the Player model
            var playerEntity = new PlayerEntity
            {
                Id = player.Id,
                SeasonId = player.SeasonId,
                UserId = player.UserId,
                Name = player.Name,
                Sex = player.Sex
            };

            _playerEntities.Add(playerEntity); // Add the new PlayerEntity
            return Task.FromResult(true); // Return success
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            // Remove all PlayerEntities with the specified ID and return whether any were removed
            var removedCount = _playerEntities.RemoveAll(pe => pe.Id == id);
            return Task.FromResult(removedCount > 0);
        }

        public Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
        {
            // Check if any PlayerEntity exists with the specified ID
            return Task.FromResult(_playerEntities.Any(pe => pe.Id == id));
        }

        public Task<IEnumerable<Player>> GetAllAsync(CancellationToken token = default)
        {
            // Convert PlayerEntities to Player models and return
            var players = _playerEntities.Select(pe => new Player { Id = pe.Id, UserId = pe.UserId, Name = pe.Name, Sex = pe.Sex, SeasonId = pe.SeasonId });
            return Task.FromResult(players.AsEnumerable());
        }

        public Task<Player?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            // Find the PlayerEntity by ID
            var playerEntity = _playerEntities.SingleOrDefault(pe => pe.Id == id);
            if (playerEntity == null) return Task.FromResult<Player?>(null); // Not found

            // Create and return a Player model from the PlayerEntity
            var player = new Player { Id = playerEntity.Id, UserId = playerEntity.UserId, Name = playerEntity.Name, Sex = playerEntity.Sex, SeasonId = playerEntity.SeasonId };
            return Task.FromResult<Player?>(player);
        }

        public Task<bool> UpdateAsync(Player player, CancellationToken token = default)
        {
            // Find the index of the PlayerEntity to update
            var playerIndex = _playerEntities.FindIndex(pe => pe.Id == player.Id);
            if (playerIndex == -1) return Task.FromResult(false); // Player not found

            // Update the existing PlayerEntity
            var existingPlayerEntity = _playerEntities[playerIndex];
            existingPlayerEntity.UserId = player.UserId; // Assuming UserId can be updated
            existingPlayerEntity.Name = player.Name;
            existingPlayerEntity.Sex = player.Sex;
            existingPlayerEntity.SeasonId = player.SeasonId;

            return Task.FromResult(true); // Return success
        }


        public Task<bool> CreateManyAsync(IEnumerable<Player> players, CancellationToken token = default)
        {
            var addedPlayers = 0;

            foreach (var player in players)
            {
                Console.WriteLine($"Processing Match: Id={player.Id}, SeasonId={player.SeasonId}, Name={player.Name}.");

                // Check if a MatchEntity with the same Id already exists
                if (_playerEntities.Any(m => m.Id == player.Id))
                {
                    Console.WriteLine($"Match with Id {player.Id} already exists, skipping.");
                    continue; // Match already exists
                }

                // Create a MatchEntity from the Match model
                var entity = new PlayerEntity
                {
                    Id = player.Id,
                    UserId = player.UserId,
                    Name = player.Name,
                    Sex = player.Sex,
                    SeasonId = player.SeasonId
                };

                // Check if the season exists (this check should be improved)
                // Example: Check against a separate seasons repository
                // if (!_seasonRepository.ExistsById(match.SeasonId)) 
                // {
                //     Console.WriteLine($"Season with Id {match.SeasonId} does not exist, skipping match.");
                //     continue; // Skip this match if the season does not exist
                // }

                // For now, let's skip this condition for testing
                _playerEntities.Add(entity); // Add the new MatchEntity
                addedPlayers++;

                Console.WriteLine($"Added Match: Id={entity.Id}, SeasonId={entity.SeasonId}");
            }

            return Task.FromResult(addedPlayers > 0);
        }

        public Task<IEnumerable<Player>> GetBySeasonIdAsync(Guid seasonId, CancellationToken token = default)
        {
            var players = _playerEntities.Where(pe => pe.SeasonId == seasonId).Select(pe => new Player { Id = pe.Id, UserId = pe.UserId, Name = pe.Name, Sex = pe.Sex, SeasonId = pe.SeasonId });
            return Task.FromResult(players.AsEnumerable());
        }

    }
}
