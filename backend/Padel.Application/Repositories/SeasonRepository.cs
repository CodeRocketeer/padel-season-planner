using Padel.Domain.Models;
using Padel.Infrastructure.Entities;
using System.Data;

namespace Padel.Application.Repositories
{
    public class SeasonRepository : ISeasonRepository
    {
        private readonly List<SeasonEntity> _seasons = new();
        private readonly IMatchRepository _matchRepository; // Dependency on MatchRepository
        private readonly ITeamRepository _teamRepository;



        public SeasonRepository(IMatchRepository matchRepository, ITeamRepository teamRepository)
        {
            _matchRepository = matchRepository; // Injecting MatchRepository
            _teamRepository = teamRepository;
        }


        public Task<bool> CreateAsync(Season season, CancellationToken token = default)
        {
            if (season.AmountOfMatches <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(season.AmountOfMatches), "Amount of matches must be greater than zero.");
            }

            var entity = new SeasonEntity
            {
                Id = season.Id,
                Name = season.Name,
                StartDate = season.StartDate,
                DayOfWeek = season.DayOfWeek,
                AmountOfMatches = season.AmountOfMatches
            };

            _seasons.Add(entity);
            return Task.FromResult(true); // Simulate success
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            var entityToRemove = _seasons.FirstOrDefault(s => s.Id == id);
            if (entityToRemove != null)
            {
                _seasons.Remove(entityToRemove);
                return Task.FromResult(true); // Simulate successful deletion
            }

            return Task.FromResult(false); // No entity found to delete
        }

        public Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
        {
            var exists = _seasons.Any(s => s.Id == id);
            return Task.FromResult(exists);
        }

        public Task<IEnumerable<Season>> GetAllAsync(CancellationToken token = default)
        {
            var result = _seasons.Select(x => new Season
            {
                Id = x.Id,
                Name = x.Name,
                StartDate = x.StartDate,
                DayOfWeek = x.DayOfWeek,
                AmountOfMatches = x.AmountOfMatches

            });

            return Task.FromResult<IEnumerable<Season>>(result.ToList());
        }

        public async Task<Season?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            var entity = _seasons.SingleOrDefault(s => s.Id == id);
            if (entity == null) return null; // Return null if the entity is not found

            // Populate the matches using async call
            var matches = await _matchRepository.GetBySeasonIdAsync(entity.Id, token);

            var teams = await _teamRepository.GetAllBySeasonIdAsync(entity.Id, token);

            // add the teams to the matches
            foreach (var match in matches)
            {
                var team1 = teams.FirstOrDefault(t => t.Id == match.Team1Id);
                var team2 = teams.FirstOrDefault(t => t.Id == match.Team2Id);

                if (team1 != null && team2 != null)
                {
                    match.Teams = new List<Team> { team1, team2 };
                }
            }


            return new Season
            {
                Id = entity.Id,
                Name = entity.Name,
                StartDate = entity.StartDate,
                DayOfWeek = entity.DayOfWeek,
                AmountOfMatches = entity.AmountOfMatches,
                Matches = matches.ToList() // Convert to List if needed
            };
        }

        public Task<bool> UpdateAsync(Season season, CancellationToken token = default)
        {
            var entityToUpdate = _seasons.FirstOrDefault(s => s.Id == season.Id);
            if (entityToUpdate != null)
            {
                entityToUpdate.Name = season.Name;
                entityToUpdate.StartDate = season.StartDate;
                entityToUpdate.DayOfWeek = season.DayOfWeek;
                entityToUpdate.AmountOfMatches = season.AmountOfMatches;
                return Task.FromResult(true); // Simulate successful update
            }

            return Task.FromResult(false); // No entity found to update
        }
    }
}