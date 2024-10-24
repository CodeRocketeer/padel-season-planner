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
        Task<bool> CreateAsync(Season Season, CancellationToken token = default);

        Task<Season?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default);

        Task<Season?> GetBySlugAsync(string slug, Guid? userid = default, CancellationToken token = default);

        Task<IEnumerable<Season>> GetAllAsync(Guid? userid = default, CancellationToken token = default);

        Task<Season?> UpdateAsync(Season Season, Guid? userid = default, CancellationToken token = default);

        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);


        Task<bool> ConfirmSeasonAsync(Guid id, Guid? userid = default, CancellationToken token = default);
    }
}
