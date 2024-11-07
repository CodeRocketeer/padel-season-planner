using Microsoft.EntityFrameworkCore;
using Padel.Infrastructure.Entities;
using Padel.Infrastructure;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Padel.Infrastructure.Repositories.Interfaces;

namespace Padel.Infrastructure.Repositories;

public class SeasonRepository : ISeasonRepository
{
    private readonly AppDbContext _context;

    public SeasonRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SeasonEntity?> GetByIdAsync(int id, CancellationToken token = default)
    {
        return await _context.Seasons
            .Include(s => s.Matches) // Include related matches
            .Include(s => s.Players) // Include players in the season
            .FirstOrDefaultAsync(s => s.Id == id, token);
    }

    public async Task<IEnumerable<SeasonEntity>> GetAllAsync(CancellationToken token = default)
    {
        return await _context.Seasons
            .Include(s => s.Matches)
            .Include(s => s.Players)
            .ToListAsync(token);
    }

    public async Task<SeasonEntity> AddAsync(SeasonEntity seasonEntity, CancellationToken token = default)
    {
        await _context.Seasons.AddAsync(seasonEntity, token);
        await _context.SaveChangesAsync(token); // Save changes here
        return seasonEntity; // Return the created entity
    }

    public async Task<SeasonEntity> UpdateAsync(SeasonEntity seasonEntity, CancellationToken token = default)
    {
        // Check if the entity exists before updating
        var existingSeason = await GetByIdAsync(seasonEntity.Id, token);
        if (existingSeason == null)
        {
            throw new KeyNotFoundException($"Season with ID {seasonEntity.Id} not found.");
        }

        // Update the existing entity with the new values
        _context.Entry(existingSeason).CurrentValues.SetValues(seasonEntity);
        await _context.SaveChangesAsync(token); // Save changes here

        return existingSeason; // Return the updated entity
    }


    public async Task<bool> DeleteAsync(int id, CancellationToken token = default)
    {
        var seasonEntity = await GetByIdAsync(id, token);
        if (seasonEntity != null)
        {
            _context.Seasons.Remove(seasonEntity);
            await _context.SaveChangesAsync(token); // Save changes here
            return true; // Deletion was successful
        }
        return false; // Entity not found, deletion was not successful
    }

    public async Task<bool> JoinPlayerToSeasonAsync(int seasonId, Guid userId, CancellationToken token)
    {
        // Fetch the PlayerEntity by userId
        var playerEntity = await _context.Players.FirstOrDefaultAsync(p => p.UserId == userId, token);
        if (playerEntity == null)
        {
            throw new KeyNotFoundException($"Player with ID {userId} not found."); // Throw an error if the player does not exist
        }

        // Fetch the SeasonEntity with the related Players
        var seasonEntity = await _context.Seasons
            .Include(s => s.Players) // Include Players navigation property
            .FirstOrDefaultAsync(s => s.Id == seasonId, token);
        if (seasonEntity == null)
        {
            throw new KeyNotFoundException($"Season with ID {seasonId} not found."); // Throw an error if the season does not exist
        }

        // Check if the player is already in the season
        if (seasonEntity.Players.Any(p => p.UserId == playerEntity.UserId))
        {
            throw new InvalidOperationException($"Player with ID {playerEntity.UserId} has already joined season {seasonId}."); // Throw an error if the player is already a participant
        }

        // Add the player to the season
        seasonEntity.Players.Add(playerEntity);

        await _context.SaveChangesAsync(token);
        return true; // Successfully joined
    }

    



}
