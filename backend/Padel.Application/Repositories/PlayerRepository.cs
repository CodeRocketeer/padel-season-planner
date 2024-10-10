using Padel.Domain.Models;

namespace Padel.Application.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {

        private readonly List<Player> _players = new();

        public Task<bool> CreateAsync(Player player)
        {
            // Check if a player with the same Id already exists
            if (_players.Any(t => t.Id == player.Id))
            {
                // Return false if a player with the same Id already exists
                return Task.FromResult(false);
            }

            // Add the player if it doesn't already exist
            _players.Add(player);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            var removedCount = _players.RemoveAll(t => t.Id == id);
            var movieRemoved = removedCount > 0;
            return Task.FromResult(movieRemoved);
        }

        public Task<IEnumerable<Player>> GetAllAsync()
        {
            return Task.FromResult(_players.AsEnumerable());
        }

        public Task<Player?> GetByIdAsync(Guid id)
        {
            var player = _players.SingleOrDefault(t => t.Id == id);

            return Task.FromResult(player);

        }

        public Task<bool> UpdateAsync(Player player)
        {
            var playerIndex = _players.FindIndex(t => t.Id == player.Id);
            if (playerIndex == -1)
            {
                return Task.FromResult(false);
            }
            _players[playerIndex] = player;
            return Task.FromResult(true);
        }
    }
}
