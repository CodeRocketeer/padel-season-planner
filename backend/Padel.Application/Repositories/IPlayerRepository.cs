using Padel.Domain.Models;

namespace Padel.Application.Repositories
{
    public interface IPlayerRepository
    {

        Task<bool> CreateAsync(Player player);

        Task<Player?> GetByIdAsync(Guid id);

        Task<IEnumerable<Player>> GetAllAsync();

        Task<bool> UpdateAsync(Player player);

        Task<bool> DeleteByIdAsync(Guid id);

    }
}
