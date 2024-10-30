using Padel.Infrastructure.Entities;
using Padel.Domain.Models;


namespace Padel.Infrastructure.Repositories.Interfaces;

public interface ISeasonRepository
{
    Task<SeasonEntity?> GetByIdAsync(int id , CancellationToken token = default);
    Task<IEnumerable<SeasonEntity>> GetAllAsync(CancellationToken token = default);
    Task<SeasonEntity> AddAsync(SeasonEntity seasonEntity, CancellationToken token = default); 
    Task<SeasonEntity> UpdateAsync(SeasonEntity seasonEntity, CancellationToken token = default);
    Task<bool> DeleteAsync(int id, CancellationToken token = default);
    Task<bool> JoinPlayerToSeasonAsync(int seasonId, Guid userId, CancellationToken token);
}
