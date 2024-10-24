using Padel.Application.Models;

namespace Padel.Application.Repositories.Interfaces
{
    public interface IMatchRepository
    {

        Task<bool> CreateManyAsync(List<Match> matches, CancellationToken token = default);

        Task<IEnumerable<Match>> GetAllAsync(GetAllMatchesOptions options, CancellationToken token = default);
    }
}
