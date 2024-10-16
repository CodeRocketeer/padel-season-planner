using Padel.Domain.Models;

namespace Padel.Application.Repositories
{
    public interface IPlayerRepository
    {

        Task<bool> CreateAsync(Player player, CancellationToken token = default);

        Task<Player?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<Player>> GetAllAsync(CancellationToken token = default);

        Task<bool> UpdateAsync(Player player, CancellationToken token = default);

        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

        Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);

        Task<bool> CreateManyAsync(IEnumerable<Player> players = null, CancellationToken token = default);

        Task<IEnumerable<Player>> GetBySeasonIdAsync(Guid seasonId, CancellationToken token = default); // IEnumerable<Player>>

    }
}
