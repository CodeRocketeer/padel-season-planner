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
    Task<Player?> GetPlayerByIdAsync(int id, CancellationToken token = default);

    // Retrieve all players
    Task<IEnumerable<Player>> GetAllPlayersAsync(GetAllPlayersOptions options , CancellationToken token = default);

    // Add a new player
    Task<Player> AddPlayerAsync(Player player, CancellationToken token = default);

    // Update an existing player
    Task<Player> UpdatePlayerAsync(Player player, CancellationToken token = default);

    // Delete a player by ID
    Task<bool> DeletePlayerAsync(int id, CancellationToken token = default);
}
