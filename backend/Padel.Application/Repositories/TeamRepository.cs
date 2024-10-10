using Padel.Application.Models;

namespace Padel.Application.Repositories
{
    public class TeamRepository : ITeamRepository
    {

        private readonly List<Team> _teams = new();

        public Task<bool> CreateAsync(Team team)
        {
            // Check if a team with the same Id already exists
            if (_teams.Any(t => t.Id == team.Id))
            {
                // Return false if a team with the same Id already exists
                return Task.FromResult(false);
            }

            // Add the team if it doesn't already exist
            _teams.Add(team);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            var removedCount = _teams.RemoveAll(t => t.Id == id);
            var movieRemoved = removedCount > 0;
            return Task.FromResult(movieRemoved);
        }

        public Task<bool> ExistsByIdAsync(Guid id)
        {
            var teamExists = _teams.Any(t => t.Id == id);

            return Task.FromResult(teamExists);
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
