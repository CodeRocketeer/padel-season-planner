using Microsoft.EntityFrameworkCore;
using Padel.Infrastructure.Entities;
using Padel.Infrastructure;
using Padel.Infrastructure.Repositories.Interfaces;

namespace Padel.Infrastructure.Repositories;


public class TeamRepository : ITeamRepository
{
    private readonly AppDbContext _context;

    public TeamRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TeamEntity?> GetByIdAsync(int id)
    {
        return await _context.Teams
            .Include(t => t.Players) // Include players in teams
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<TeamEntity>> GetAllAsync()
    {
        return await _context.Teams
            .Include(t => t.Players)
            .ToListAsync();
    }

    public async Task AddAsync(TeamEntity teamEntity)
    {
        await _context.Teams.AddAsync(teamEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TeamEntity teamEntity)
    {
        _context.Teams.Update(teamEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var teamEntity = await GetByIdAsync(id);
        if (teamEntity != null)
        {
            _context.Teams.Remove(teamEntity);
            await _context.SaveChangesAsync();
        }
    }
}


