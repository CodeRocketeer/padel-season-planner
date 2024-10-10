
using Padel.Application.Models;

namespace Padel.Application.Repositories
{
    public interface ITeamRepository
    {

        Task<bool> CreateAsync(Team team);

        Task<Team?> GetByIdAsync(Guid id);

        Task<IEnumerable<Team>> GetAllAsync();

        Task<bool> UpdateAsync(Team team);

        Task<bool> DeleteByIdAsync(Guid id);

        Task<bool> ExistsByIdAsync(Guid id);

    }
}
