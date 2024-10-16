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
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IValidator<Team> _teamValidator;
        private readonly IValidator<Match> _matchValidator;
        private readonly ITeamService _teamService;


        public SeasonService(ISeasonRepository seasonRepository, IMatchService matchService, IMatchRepository matchRepository, IValidator<Season> seasonValidator, IPlayerRepository playerRepository, ITeamRepository teamRepository, IValidator<Team> teamValidator, IValidator<Match> matchValidator, ITeamService teamService)
        {
            _seasonRepository = seasonRepository;
            _matchService = matchService;
            _seasonValidator = seasonValidator;
            _matchRepository = matchRepository;
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
            _teamValidator = teamValidator;
            _teamService = teamService;
            _matchValidator = matchValidator;
        }

        public async Task<bool> CreateAsync(Season season, CancellationToken token = default)
        {
            // Validate and create the season first
            await _seasonValidator.ValidateAndThrowAsync(season, token);
            var isCreated = await _seasonRepository.CreateAsync(season, token);

            if (!isCreated) return false;
            return true;
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


        public async Task<bool> PopulateSeasonAsync(Guid id, CancellationToken token = default)
        {
            // Check if the season exists
            var season = await _seasonRepository.GetByIdAsync(id, token);
            if (season == null) return false;

            // if the season is already populated then return false
            if (season.Matches.Count == season.AmountOfMatches)
            {
                throw new InvalidOperationException("Season is already populated");
            }

            // Get players for the season
            var players = await _playerRepository.GetBySeasonIdAsync(id, token);
            if (players == null || !players.Any()) return false;

            // Generate all possible teams for the season
            var teams = await _teamService.GenerateTeamsForSeason(season, players, token);
            var matches = await _matchService.GenerateBalancedMatchesForSeason(teams, season, players.ToList(), token);

            var usedTeamIds = matches.SelectMany(m => new[] { m.Team1Id, m.Team2Id }).Distinct().ToList();
            var usedTeams = teams.Where(t => usedTeamIds.Contains(t.Id)).ToList();

            // Validate and save the used teams
            foreach (var team in usedTeams)
            {
                await _teamValidator.ValidateAndThrowAsync(team, token);
            }

            await _teamRepository.CreateManyAsync(usedTeams, token);
            await _matchRepository.CreateManyAsync(matches, token);

            return true;
        }

    }


}
