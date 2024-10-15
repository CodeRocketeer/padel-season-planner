using FluentValidation;
using Padel.Application.Repositories;
using Padel.Domain.Models;

namespace Padel.Application.Services
{
    public class SeasonService : ISeasonService
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly IValidator<Season> _seasonValidator;
        private readonly IMatchService _matchService;
        private readonly IMatchRepository _matchRepository;

        public SeasonService(ISeasonRepository seasonRepository, IMatchService matchService, IMatchRepository matchRepository, IValidator<Season> seasonValidator)
        {
            _seasonRepository = seasonRepository;
            _matchService = matchService;
            _seasonValidator = seasonValidator;
            _matchRepository = matchRepository;
        }

        public async Task<bool> CreateAsync(Season season, CancellationToken token = default)
        {
            // Validate and create the season first
            await _seasonValidator.ValidateAndThrowAsync(season, token);
            var isCreated = await _seasonRepository.CreateAsync(season, token);
            if (!isCreated) return false;

            // Generate matches based on the season's data
            var matches = GenerateMatchesForSeason(season);

            // Create matches using the new bulk insert method in the match repository
            var matchesCreated = await _matchRepository.CreateMatchesAsync(matches, token);

            return matchesCreated; // Return true if the season and matches are created
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _seasonRepository.DeleteByIdAsync(id, token);
        }

        public Task<IEnumerable<Season>> GetAllAsync(CancellationToken token = default)
        {
            return _seasonRepository.GetAllAsync(token);
        }

        public Task<Season?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _seasonRepository.GetByIdAsync(id, token);
        }

        public async Task<Season?> UpdateAsync(Season season, CancellationToken token = default)
        {
            // Season validation occurs in the Season model
            await _seasonValidator.ValidateAndThrowAsync(season, token);
            var seasonExists = await _seasonRepository.ExistsByIdAsync(season.Id, token);
            if (!seasonExists) return null;

            await _seasonRepository.UpdateAsync(season, token);
            return season;
        }

        private List<Match> GenerateMatchesForSeason(Season season)
        {
            var matches = new List<Match>();
            var currentMatchDate = GetFirstMatchDate(season.StartDate, season.DayOfWeek);

            for (int i = 0; i < season.AmountOfMatches; i++)
            {
                var match = new Match
                {
                    Id = Guid.NewGuid(),
                    SeasonId = season.Id,
                    MatchDate = currentMatchDate
                };

                matches.Add(match);

                // Schedule the next match for the following week
                currentMatchDate = currentMatchDate.AddDays(7);
            }

            return matches;
        }


        private DateTime GetFirstMatchDate(DateTime startDate, int dayOfWeek)
        {
            // Get the difference between the current day of the week and the desired match day
            int daysUntilMatchDay = ((dayOfWeek - (int)startDate.DayOfWeek + 7) % 7);

            // Return the first match date, which is the start date plus the calculated difference
            return startDate.AddDays(daysUntilMatchDay);
        }

    }


}
