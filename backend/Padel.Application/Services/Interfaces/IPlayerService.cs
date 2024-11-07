using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Padel.Shared.Options;
using Padel.Domain.Models;

namespace Padel.Application.Services.Interfaces;

public interface IPlayerService
{

    // Retrieve a player by their ID
    Task<Player?> GetPlayerByIdAsync(Guid userId, CancellationToken token = default);

    // Retrieve all players
    Task<IEnumerable<Player>> GetAllPlayersAsync(GetAllPlayersOptions options , CancellationToken token = default);

    // Add a new player
    Task<Player> AddPlayerAsync(Player player, CancellationToken token = default);


}
