using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Repositories
{
    public interface ITeamRepository
    {

        Task<bool> CreateAsync(Team team);

        Task<Team?> GetByIdAsync(Guid id);

        Task<IEnumerable<Team>> GetAllAsync();

        Task<bool> UpdateAsync(Team team);

        Task<bool> DeleteByIdAsync(Guid id);

    }
}
