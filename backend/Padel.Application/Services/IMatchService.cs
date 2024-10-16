
using Padel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Services
{
    public interface IMatchService
    {

        Task<bool> CreateAsync(Match match, CancellationToken token = default);

        Task<Match?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<Match>> GetAllAsync(CancellationToken token = default);

        Task<Match?> UpdateAsync(Match match, CancellationToken token = default);

        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<Match>> GetAllBySeasonIdAsync(Guid seasonId, CancellationToken token = default);

        Task<IEnumerable<Match>> GenerateBalancedMatchesForSeason(List<Team> teams, Season season, List<Player> players, CancellationToken token = default);

    }
}
