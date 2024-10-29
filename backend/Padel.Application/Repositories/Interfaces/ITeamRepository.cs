using Padel.Application.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Repositories.Interfaces;

public interface ITeamRepository
{
    Task<TeamEntity?> GetByIdAsync(int id);
    Task<IEnumerable<TeamEntity>> GetAllAsync();
    Task AddAsync(TeamEntity teamEntity);
    Task UpdateAsync(TeamEntity teamEntity);
    Task DeleteAsync(int id);
}
