

using Padel.Domain.Models;

namespace Padel.Application.Repositories
{
    public interface ITeamRepository
    {

        Task<bool> CreateAsync(Team team, CancellationToken token = default);

        Task<Team?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<Team>> GetAllAsync(CancellationToken token = default);

        Task<bool> UpdateAsync(Team team, CancellationToken token = default);

        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

        Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);

    }
}
