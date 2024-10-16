using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Padel.Domain.Models;
using Padel.Application.Repositories;

namespace Padel.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<bool> CreateAsync(Team team, CancellationToken token = default)
        {
            // Team validation occurs in the Team model
            return await _teamRepository.CreateAsync(team, token);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _teamRepository.DeleteByIdAsync(id, token);
        }

        public Task<IEnumerable<Team>> GetAllAsync(CancellationToken token = default)
        {
            return _teamRepository.GetAllAsync(token);
        }

        public Task<Team?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _teamRepository.GetByIdAsync(id, token);
        }

        public async Task<Team?> UpdateAsync(Team team, CancellationToken token = default)
        {
            // Team validation occurs in the Team model
            var teamExists = await _teamRepository.ExistsByIdAsync(team.Id,token);
            if (!teamExists) return null;

            await _teamRepository.UpdateAsync(team, token);
            return team;
        }

        public Task<IEnumerable<Team>> GetAllBySeasonIdAsync(Guid seasonId, CancellationToken token = default)
        {
            return _teamRepository.GetAllBySeasonIdAsync(seasonId, token);
        }

        // non repository methods
        // Generate all unique pairs of players to create teams (CHECKED)
        public List<Team> GenerateTeamsForSeason(Season season, IEnumerable<Player> players)
        {
            var teams = new List<Team>();
            var playerList = players.ToList();

            // Generate all unique pairs of players
            for (int i = 0; i < playerList.Count; i++)
            {
                for (int j = i + 1; j < playerList.Count; j++)
                {
                    var team = new Team
                    {
                        Id = Guid.NewGuid(),
                        SeasonId = season.Id,
                        Player1Id = playerList[i].Id,
                        Player2Id = playerList[j].Id,
                        Players = new List<Player>() { playerList[i], playerList[j] }
                    };

                    teams.Add(team);
                }
            }

            // Validate the number of generated teams
            ValidateGeneratedTeams(playerList.Count, teams);

            return teams;
        }

        
        private void ValidateGeneratedTeams(int playerCount, List<Team> teams)
        {
            // Calculate the expected number of teams using the combination formula: C(n, 2) = n(n-1)/2
            int expectedTeamCount = (playerCount * (playerCount - 1)) / 2;

            // Check if the generated number of teams matches the expected number
            if (teams.Count != expectedTeamCount)
            {
                throw new InvalidOperationException($"Invalid number of teams generated. Expected {expectedTeamCount} teams, but generated {teams.Count} teams.");
            }
        }



    }
}
