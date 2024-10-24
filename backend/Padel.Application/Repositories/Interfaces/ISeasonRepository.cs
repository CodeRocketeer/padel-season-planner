using Padel.Application.Models;


namespace Padel.Application.Repositories.Interfaces;

public interface ISeasonRepository
{
    Task<bool> CreateAsync(Season Season, CancellationToken token = default);

    Task<Season?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default);

    Task<Season?> GetBySlugAsync(string slug, Guid? userid = default, CancellationToken token = default);

    Task<IEnumerable<Season>> GetAllAsync(Guid? userid = default, CancellationToken token = default);

    Task<bool> UpdateAsync(Season Season, Guid? userid = default, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
}
