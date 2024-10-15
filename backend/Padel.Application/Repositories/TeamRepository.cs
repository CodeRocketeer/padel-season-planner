using Padel.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Padel.Application.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly List<Team> _teams = new();

        public Task<bool> CreateAsync(Team team, CancellationToken token = default)
        {
            if (_teams.Any(t => t.Id == team.Id))
            {
                return Task.FromResult(false); // Team already exists
            }

            _teams.Add(team);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            var removedCount = _teams.RemoveAll(t => t.Id == id);
            return Task.FromResult(removedCount > 0);
        }

        public Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
        {
            return Task.FromResult(_teams.Any(t => t.Id == id));
        }

        public Task<IEnumerable<Team>> GetAllAsync(CancellationToken token = default)
        {
            return Task.FromResult(_teams.AsEnumerable());
        }

        public Task<Team?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return Task.FromResult(_teams.SingleOrDefault(t => t.Id == id));
        }

        public Task<bool> UpdateAsync(Team team, CancellationToken token = default)
        {
            var teamIndex = _teams.FindIndex(t => t.Id == team.Id);
            if (teamIndex == -1) return Task.FromResult(false);

            _teams[teamIndex] = team;
            return Task.FromResult(true);
        }
    }
}
