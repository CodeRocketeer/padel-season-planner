using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Repositories
{
    public class TeamRepository : ITeamRepository
    {

        private readonly List<Team> _teams = new();

        public Task<bool> CreateAsync(Team team)
        {
           _teams.Add(team);

            return Task.FromResult(true);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            var removedCount = _teams.RemoveAll(t => t.Id == id);
            var movieRemoved = removedCount > 0;
            return Task.FromResult(movieRemoved);
        }

        public Task<IEnumerable<Team>> GetAllAsync()
        {
           return Task.FromResult(_teams.AsEnumerable());
        }

        public Task<Team?> GetByIdAsync(Guid id)
        {
            var team = _teams.SingleOrDefault(t => t.Id == id);

            return Task.FromResult(team);

        }

        public Task<bool> UpdateAsync(Team team)
        {
            var teamIndex = _teams.FindIndex(t => t.Id == team.Id);
            if (teamIndex == -1)
            { 
                return Task.FromResult(false);
            }
            _teams[teamIndex] = team;
            return Task.FromResult(true);
        }
    }
}
