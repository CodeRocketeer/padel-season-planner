using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Services.Interfaces
{
    public interface ISeasonService
    {
        Task<Season> CreateAsync(Season Season, CancellationToken token = default);

        Task<Season?> GetByIdAsync(int id, CancellationToken token = default);

        Task<IEnumerable<Season>> GetAllAsync( CancellationToken token = default);
        Task<bool> DeleteByIdAsync(int id, CancellationToken token = default);

        Task<Season> UpdateAsync(Season Season, CancellationToken token = default);

        Task<bool> JoinSeasonAsync(int seasonId, Guid userId, CancellationToken token);



    }
}
