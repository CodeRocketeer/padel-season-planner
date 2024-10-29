using Padel.Application.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Repositories.Interfaces;

public interface IMatchRepository
{
    Task<MatchEntity> GetByIdAsync(int id);
    Task<IEnumerable<MatchEntity>> GetAllAsync();
    Task AddAsync(MatchEntity matchEntity);
    Task UpdateAsync(MatchEntity matchEntity);
    Task DeleteAsync(int id);
}
