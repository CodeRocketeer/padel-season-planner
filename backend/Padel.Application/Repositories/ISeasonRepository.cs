using Padel.Domain.Models;

namespace Padel.Application.Repositories
{
    public interface ISeasonRepository
    {

        Task<bool> CreateAsync(Season season, CancellationToken token = default);

        Task<Season?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<Season>> GetAllAsync(CancellationToken token = default);

        Task<bool> UpdateAsync(Season season, CancellationToken token = default);

        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

        Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
    }

}
