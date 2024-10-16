
using Padel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Services
{
    public interface IPlayerService
    {

        Task<bool> CreateAsync(Player player, CancellationToken token = default);

        Task<Player?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<IEnumerable<Player>> GetAllAsync( CancellationToken token = default);

        Task<Player?> UpdateAsync(Player player, CancellationToken token = default);

        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default) ;

        Task<bool> SeedPlayersAsync(Guid seasonId , int amountOfPlayers, CancellationToken token = default);

    }
}
