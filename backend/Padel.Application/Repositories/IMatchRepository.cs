using Padel.Domain.Models;

namespace Padel.Application.Repositories
{
    public interface IMatchRepository
    {


        Task<bool> CreateAsync(Match match, CancellationToken token = default);

        Task<Match?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<Match>> GetAllAsync(CancellationToken token = default);

        Task<bool> UpdateAsync(Match match, CancellationToken token = default);

        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

        Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<Match>> GetBySeasonIdAsync(Guid seasonId, CancellationToken token = default);

        Task<bool> CreateManyAsync(IEnumerable<Match> matches, CancellationToken token = default);
    }



}
