using Padel.Domain.Models;
using Padel.Infrastructure.Entities;

namespace Padel.Application.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        // Using a collection of TeamEntity instead of Team
        private readonly List<TeamEntity> _teamEntities = new();

        public Task<bool> CreateAsync(Team team, CancellationToken token = default)
        {
            // Check if a TeamEntity with the same Id already exists
            if (_teamEntities.Any(t => t.Id == team.Id))
            {
                return Task.FromResult(false); // Team already exists
            }

            // Create a TeamEntity from the Team model
            var teamEntity = new TeamEntity
            {
                Id = team.Id,
                SeasonId = team.SeasonId,
                // Assuming PlayerEntity is defined with just an Id for simplicity
                Players = new List<PlayerEntity>
                {
                    new PlayerEntity { Id = team.Player1Id },
                    new PlayerEntity { Id = team.Player2Id }
                }
            };

            _teamEntities.Add(teamEntity); // Add the new TeamEntity
            return Task.FromResult(true); // Return success
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            // Remove all TeamEntities with the specified ID and return whether any were removed
            var removedCount = _teamEntities.RemoveAll(t => t.Id == id);
            return Task.FromResult(removedCount > 0);
        }

        public Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
        {
            // Check if any TeamEntity exists with the specified ID
            return Task.FromResult(_teamEntities.Any(t => t.Id == id));
        }

        public Task<IEnumerable<Team>> GetAllAsync(CancellationToken token = default)
        {
            // Convert TeamEntities to Team models and return
            var teams = _teamEntities.Select(te => new Team { Id = te.Id, SeasonId = te.SeasonId, Player1Id = te.Players[0].Id, Player2Id = te.Players[1].Id });
            return Task.FromResult(teams.AsEnumerable());
        }

        public Task<Team?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            // Find the TeamEntity by ID
            var teamEntity = _teamEntities.SingleOrDefault(t => t.Id == id);
            if (teamEntity == null) return Task.FromResult<Team?>(null); // Not found

            // Create and return a Team model from the TeamEntity
            var team = new Team { Id = teamEntity.Id, SeasonId = teamEntity.SeasonId, Player1Id = teamEntity.Players[0].Id, Player2Id = teamEntity.Players[1].Id };
            return Task.FromResult<Team?>(team);
        }

        public Task<bool> UpdateAsync(Team team, CancellationToken token = default)
        {
            // Find the index of the TeamEntity to update
            var teamIndex = _teamEntities.FindIndex(t => t.Id == team.Id);
            if (teamIndex == -1) return Task.FromResult(false); // Team not found

            // Update the existing TeamEntity
            var existingTeamEntity = _teamEntities[teamIndex];
            existingTeamEntity.SeasonId = team.SeasonId; // Assuming SeasonId can be updated
            existingTeamEntity.Players.Clear();
            existingTeamEntity.Players.Add(new PlayerEntity { Id = team.Player1Id });
            existingTeamEntity.Players.Add(new PlayerEntity { Id = team.Player2Id });

            return Task.FromResult(true); // Return success
        }

        public Task<bool> CreateManyAsync(IEnumerable<Team> teams, CancellationToken token = default)
        {
            var addedTeams = 0;

            foreach (var team in teams)
            {
                Console.WriteLine($"Processing Team: Id={team.Id}, SeasonId={team.SeasonId}, Player1Id={team.Player1Id}, Player2Id={team.Player2Id}.");

                // Check if a TeamEntity with the same Id already exists
                if (_teamEntities.Any(t => t.Id == team.Id))
                {
                    Console.WriteLine($"Team with Id {team.Id} already exists, skipping.");
                    continue; // Team already exists
                }

                var teamEntity = new TeamEntity
                {
                    Id = team.Id,
                    SeasonId = team.SeasonId,
                    Players = new List<PlayerEntity> {
                        new PlayerEntity { Id = team.Player1Id },
                        new PlayerEntity { Id = team.Player2Id }
                    }
                };

                _teamEntities.Add(teamEntity); // Add the new TeamEntity
                addedTeams++;
            }

            return Task.FromResult(addedTeams > 0);
        }


        public Task<IEnumerable<Team>> GetAllBySeasonIdAsync(Guid seasonId, CancellationToken token = default)
        {
            var teams = _teamEntities.Where(te => te.SeasonId == seasonId).Select(te => new Team { Id = te.Id, SeasonId = te.SeasonId, Player1Id = te.Players[0].Id, Player2Id = te.Players[1].Id });
            return Task.FromResult(teams);
        }
   

    
    }
}

