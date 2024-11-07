using Microsoft.EntityFrameworkCore;
using Padel.Infrastructure.Entities;
using Padel.Infrastructure;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Padel.Infrastructure.Repositories.Interfaces;
using Padel.Shared.Options;
using Microsoft.Extensions.Logging;

namespace Padel.Infrastructure.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<PlayerRepository> _logger;

    public PlayerRepository(AppDbContext context, ILogger<PlayerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<PlayerEntity?> GetByUserIdAsync(Guid userId, CancellationToken token = default)
    {
        try
        {
            return await _context.Players
                .FirstOrDefaultAsync(p => p.UserId == userId, token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching player by UserId.");
            throw new Exception("An error occurred while fetching player details.", ex);
        }
    }

    public async Task<IEnumerable<PlayerEntity>> GetAllAsync(GetAllPlayersOptions options, CancellationToken token = default)
    {
        try
        {
            IQueryable<PlayerEntity> query = _context.Players;

            if (options.SeasonId.HasValue)
            {
                query = query.Where(p => p.Seasons.Any(s => s.Id == options.SeasonId.Value));
            }

            return await query.ToListAsync(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching players.");
            throw new Exception("An error occurred while fetching players.", ex);
        }
    }

    public async Task<PlayerEntity> AddAsync(PlayerEntity playerEntity, CancellationToken token = default)
    {
        try
        {
            await _context.Players.AddAsync(playerEntity, token);
            await _context.SaveChangesAsync(token); // Ensure changes are saved
            return playerEntity; // Return the created PlayerEntity
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Error adding player.");
            throw new Exception("An error occurred while adding the player.", dbEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while adding player.");
            throw new Exception("An unexpected error occurred while adding the player.", ex);
        }
    }

    public async Task<PlayerEntity> UpdateAsync(PlayerEntity playerEntity, CancellationToken token = default)
    {
        try
        {
            _context.Players.Update(playerEntity); // Mark the entity as modified
            await _context.SaveChangesAsync(token); // Save changes
            return playerEntity; // Return the updated PlayerEntity
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Error updating player.");
            throw new Exception("An error occurred while updating the player.", dbEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating player.");
            throw new Exception("An unexpected error occurred while updating the player.", ex);
        }
    }

    public async Task<bool> DeleteAsync(Guid userId, CancellationToken token = default)
    {
        try
        {
            var playerEntity = await GetByUserIdAsync(userId, token);
            if (playerEntity == null)
            {
                throw new KeyNotFoundException($"Player with UserId {userId} not found.");
            }

            _context.Players.Remove(playerEntity); // Remove the player
            await _context.SaveChangesAsync(token); // Save changes
            return true; // Return true if deletion was successful
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogWarning(knfEx, "Player not found for deletion.");
            return false; // Or you could throw the exception and handle it elsewhere
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Error deleting player.");
            throw new Exception("An error occurred while deleting the player.", dbEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting player.");
            throw new Exception("An unexpected error occurred while deleting the player.", ex);
        }
    }
}
