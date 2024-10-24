using Padel.Application.Models;

namespace Padel.Application.Repositories.Interfaces
{
    public interface ITeamRepository
    {

        Task<bool> CreateManyAsync(List<Team> teams, CancellationToken token = default);
    }
}
