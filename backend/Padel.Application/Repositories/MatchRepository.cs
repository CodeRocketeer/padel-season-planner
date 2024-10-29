using Microsoft.EntityFrameworkCore;
using Padel.Application.Database;
using Padel.Application.Database.Entities;
using Padel.Application.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Repositories;

public class MatchRepository : IMatchRepository
{
    private readonly AppDbContext _context;

    public MatchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MatchEntity> GetByIdAsync(int id)
    {
        return await _context.Matches
            .Include(m => m.Teams) // Include related teams
            .ThenInclude(t => t.Players) // Include players in teams
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<MatchEntity>> GetAllAsync()
    {
        return await _context.Matches
            .Include(m => m.Teams)
            .ThenInclude(t => t.Players)
            .ToListAsync();
    }

    public async Task AddAsync(MatchEntity matchEntity)
    {
        await _context.Matches.AddAsync(matchEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MatchEntity matchEntity)
    {
        _context.Matches.Update(matchEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var matchEntity = await GetByIdAsync(id);
        if (matchEntity != null)
        {
            _context.Matches.Remove(matchEntity);
            await _context.SaveChangesAsync();
        }
    }

}
