using FluentValidation;
using Padel.Application.Models;
using Padel.Application.Repositories.Interfaces;
using Padel.Application.Services.Interfaces;

namespace Padel.Application.Services;

public class SeasonService : ISeasonService
{
    private readonly ISeasonRepository _SeasonRepository;
    private readonly IValidator<Season> _SeasonValidator;
    private readonly IParticipantRepository _participantRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ITeamService _teamService;
    private readonly IMatchService _matchService;

    public SeasonService(ISeasonRepository SeasonRepository, IValidator<Season> SeasonValidator, IParticipantRepository participantRepository, ITeamService teamService, IMatchService matchService, ITeamRepository teamRepository, IMatchRepository matchRepository)
    {
        _SeasonRepository = SeasonRepository;
        _SeasonValidator = SeasonValidator;
        _participantRepository = participantRepository;
        _teamRepository = teamRepository;
        _matchRepository = matchRepository;
        _teamService = teamService;
        _matchService = matchService;

    }

    public async Task<bool> CreateAsync(Season season, CancellationToken token = default)
    {
        await _SeasonValidator.ValidateAndThrowAsync(season, cancellationToken: token);
        return await _SeasonRepository.CreateAsync(season, token);
    }

    public Task<Season?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default)
    {
        return _SeasonRepository.GetByIdAsync(id, userid, token);
    }

    public Task<Season?> GetBySlugAsync(string slug, Guid? userid = default, CancellationToken token = default)
    {
        return _SeasonRepository.GetBySlugAsync(slug, userid, token);
    }

    public Task<IEnumerable<Season>> GetAllAsync(Guid? userid = default, CancellationToken token = default)
    {
        return _SeasonRepository.GetAllAsync(userid, token);
    }

    public async Task<Season?> UpdateAsync(Season season, Guid? userid = default, CancellationToken token = default)
    {
        await _SeasonValidator.ValidateAndThrowAsync(season, cancellationToken: token);
        var SeasonExists = await _SeasonRepository.ExistsByIdAsync(season.Id, token);
        if (!SeasonExists)
        {
            return null;
        }

        await _SeasonRepository.UpdateAsync(season, userid, token);
        return season;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _SeasonRepository.DeleteByIdAsync(id, token);
    }

    public async Task<bool> ConfirmSeasonAsync(Guid id, Guid? userid = default, CancellationToken token = default)
    {
        var season = await _SeasonRepository.GetByIdAsync(id, userid, token) ?? throw new DirectoryNotFoundException(message: $"Season with ID '{id}' does not exist.");


        // check if the season is already confirmed and populated with matches, and if so return false
        var options = new GetAllParticipantsOptions()
        {
            SeasonId = id
        };

        var participants = await _participantRepository.GetAllAsync(options, token);
        if (participants == null || participants.Count() < 2)
        {
            throw new DirectoryNotFoundException("The season does not have enough participants to generate a schedule. Please add more participants (at least 2).");
        }

        // Generate all possible teams for the season
        var teams = await _teamService.CreateTeamCombinationsForSeasonAsync(seasonId: id, participants, token);
        var matches = await _matchService.CreateBalancedMatchesForSeasonAsync(teams, season, participants, token);

        var usedTeamIds = matches.SelectMany(m => new[] { m.Team1.Id, m.Team2.Id }).Distinct().ToList();
        var usedTeams = teams.Where(t => usedTeamIds.Contains(t.Id)).ToList();

        if (matches.Count() != season.AmountOfMatches)
        {
            return false;
        }

        await _teamRepository.CreateManyAsync(usedTeams, token);
        await _matchRepository.CreateManyAsync(matches.ToList(), token);
        return true;



    }





}
