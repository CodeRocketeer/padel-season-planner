using Padel.Application.Generators;
using Padel.Application.Mappers;
using Padel.Application.Services.Interfaces;
using Padel.Domain.Models;
using Padel.Infrastructure.Repositories.Interfaces;
using Padel.Shared.Options;

namespace Padel.Application.Services;

public class SeasonService : ISeasonService
{
    private readonly ISeasonRepository _seasonRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly SeasonScheduleGenerator _seasonScheduleGenerator;

    public SeasonService(ISeasonRepository seasonRepository, IPlayerRepository playerRepository, SeasonScheduleGenerator seasonScheduleGenerator)
    {
        _seasonRepository = seasonRepository;
        _playerRepository = playerRepository;
        _seasonScheduleGenerator = seasonScheduleGenerator;
    }

    public async Task<Season> CreateAsync(Season season, CancellationToken token)
    {
        // Convert Season model to SeasonEntity using the static method
        var seasonEntity = season.ToEntity();
        // Add to repository and return the created entity's ID
        var result = await _seasonRepository.AddAsync(seasonEntity, token);
        return result.FromEntity(); // Assuming AddAsync returns SeasonEntity
    }


    public async Task<bool> DeleteByIdAsync(int id, CancellationToken token)
    {
        // Call the repository to delete the season by ID
        return await _seasonRepository.DeleteAsync(id, token);
    }

    public async Task<IEnumerable<Season>> GetAllAsync(CancellationToken token)
    {
        // Get all seasons from the repository
        var seasonEntities = await _seasonRepository.GetAllAsync(token);
        // Convert each SeasonEntity to Season model
        var seasons = seasonEntities.Select(seasonEntity => seasonEntity.FromEntity());
        return seasons;
    }

    public async Task<Season?> GetByIdAsync(int id, CancellationToken token)
    {
        // Get the season entity from the repository
        var seasonEntity = await _seasonRepository.GetByIdAsync(id, token);
        // Convert to Season model if found
        return seasonEntity != null ? seasonEntity.FromEntity() : null;
    }

    public async Task<Season> UpdateAsync(Season season, CancellationToken token)
    {
        // Convert Season model to SeasonEntity using the static method
        var seasonEntity = season.ToEntity();
        // Update the season in the repository
        var updatedEntity = await _seasonRepository.UpdateAsync(seasonEntity, token);
        // Return the updated Season model
        return updatedEntity.FromEntity();
    }

    public async Task<bool> JoinSeasonAsync(int seasonId, Guid userId, CancellationToken token)
    {
        // This method should contain the logic to add a record in the playerSeasons pivot table
        return await _seasonRepository.JoinPlayerToSeasonAsync(seasonId, userId, token);
    }

    public async Task<Season?> CreateSeasonScheduleAsync(int seasonId, CancellationToken token)
    {
        var seasonEntity = await _seasonRepository.GetByIdAsync(seasonId, token);
        if (seasonEntity == null) return null;
        Season season = seasonEntity.FromEntity();

        GetAllPlayersOptions options = new GetAllPlayersOptions();
        options.SeasonId = seasonId;

        var playersEntities = await _playerRepository.GetAllAsync(options, token);
        if (playersEntities == null)
        {
            throw new Exception("No players for season found.");
        }

        List<Player> players = new List<Player>();
        foreach (var playerEntity in playersEntities)
        {
            players.Add(playerEntity.FromEntity());
        }

        Season seasonSchedule = await _seasonScheduleGenerator.Generate(season, players);
        return seasonSchedule;
    }
}
