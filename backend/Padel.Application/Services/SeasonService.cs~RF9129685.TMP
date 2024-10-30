using Padel.Application.Mappers;
using Padel.Application.Repositories.Interfaces;
using Padel.Application.Services.Interfaces;
using Padel.Domain.Models;

namespace Padel.Application.Services;

public class SeasonService : ISeasonService
{
    private readonly ISeasonRepository _seasonRepository;

    public SeasonService(ISeasonRepository seasonRepository)
    {
        _seasonRepository = seasonRepository;
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
}
