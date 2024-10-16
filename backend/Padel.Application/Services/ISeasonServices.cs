
using Padel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Services
{
    public interface ISeasonService
    {


        Task<bool> CreateAsync(Season season, CancellationToken token = default);

        Task<Season?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<Season>> GetAllAsync(CancellationToken token = default);

        Task<Season?> UpdateAsync(Season season, CancellationToken token = default);

        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

        Task<bool> PopulateSeasonAsync(Guid id, CancellationToken token = default);
    }
}
