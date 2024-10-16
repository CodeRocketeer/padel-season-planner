
using Padel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Services
{
    public interface ITeamService
    {


        Task<bool> CreateAsync(Team team, CancellationToken token = default);

        Task<Team?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<Team>> GetAllAsync(CancellationToken token = default);

        Task<Team?> UpdateAsync(Team team, CancellationToken token = default);

        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
        List<Team> GenerateTeamsForSeason(Season season, IEnumerable<Player> players);

        Task<IEnumerable<Team>> GetAllBySeasonIdAsync(Guid seasonId, CancellationToken token = default); // <--->
    }
}
