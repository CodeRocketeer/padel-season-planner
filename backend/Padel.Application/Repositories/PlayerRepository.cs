using Microsoft.EntityFrameworkCore;
using Padel.Application.Database;
using Padel.Application.Database.Entities;
using Padel.Application.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Padel.Application.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly AppDbContext _context;

        public PlayerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PlayerEntity?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _context.Players
                .Include(p => p.Teams) // Include related teams if necessary
                .FirstOrDefaultAsync(p => p.Id == id, token);
        }

        public async Task<IEnumerable<PlayerEntity>> GetAllAsync(CancellationToken token = default)
        {
            return await _context.Players
                .Include(p => p.Teams) // Include teams if needed
                .ToListAsync(token);
        }

        public async Task<PlayerEntity> AddAsync(PlayerEntity playerEntity, CancellationToken token = default)
        {
            await _context.Players.AddAsync(playerEntity, token);
            await _context.SaveChangesAsync(token); // Ensure changes are saved
            return playerEntity; // Return the created PlayerEntity
        }

        public async Task<PlayerEntity> UpdateAsync(PlayerEntity playerEntity, CancellationToken token = default)
        {
            _context.Players.Update(playerEntity); // Mark the entity as modified
            await _context.SaveChangesAsync(token); // Save changes
            return playerEntity; // Return the updated PlayerEntity
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken token = default)
        {
            var playerEntity = await GetByIdAsync(id, token);
            if (playerEntity != null)
            {
                _context.Players.Remove(playerEntity); // Remove the player
                await _context.SaveChangesAsync(token); // Save changes
                return true; // Return true if deletion was successful
            }
            return false; // Return false if player was not found
        }
    }
}
