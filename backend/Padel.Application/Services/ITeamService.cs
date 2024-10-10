using Padel.Application.Models;
using Padel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Services
{
    public interface ITeamService
    {


        Task<bool> CreateAsync(Team team);

        Task<Team?> GetByIdAsync(Guid id);

        Task<IEnumerable<Team>> GetAllAsync();

        Task<Team?> UpdateAsync(Team team);

        Task<bool> DeleteByIdAsync(Guid id);
    }
}
