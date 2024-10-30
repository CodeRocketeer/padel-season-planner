using Padel.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Infrastructure.Repositories.Interfaces;

public interface IMatchRepository
{
    Task<MatchEntity?> GetByIdAsync(int id);
    Task<IEnumerable<MatchEntity>> GetAllAsync();
    Task AddAsync(MatchEntity matchEntity);
    Task UpdateAsync(MatchEntity matchEntity);
    Task DeleteAsync(int id);
}
