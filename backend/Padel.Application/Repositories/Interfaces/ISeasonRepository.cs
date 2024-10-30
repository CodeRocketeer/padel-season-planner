using Padel.Application.Database.Entities;
using Padel.Domain.Models;


namespace Padel.Application.Repositories.Interfaces;

public interface ISeasonRepository
{
    Task<SeasonEntity?> GetByIdAsync(int id , CancellationToken token = default);
    Task<IEnumerable<SeasonEntity>> GetAllAsync(CancellationToken token = default);
    Task<SeasonEntity> AddAsync(SeasonEntity seasonEntity, CancellationToken token = default); 
    Task<SeasonEntity> UpdateAsync(SeasonEntity seasonEntity, CancellationToken token = default);
    Task<bool> DeleteAsync(int id, CancellationToken token = default);
    Task<bool> JoinPlayerToSeasonAsync(int seasonId, Guid userId, CancellationToken token);
}
