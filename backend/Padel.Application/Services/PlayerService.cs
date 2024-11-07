using Padel.Application.Mappers;
using Padel.Infrastructure.Repositories.Interfaces;
using Padel.Application.Services.Interfaces;
using Padel.Domain.Models;
using Padel.Shared.Options;

namespace Padel.Application.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<Player> AddPlayerAsync(Player player, CancellationToken token)
    {
        GetAllPlayersOptions options = new GetAllPlayersOptions();
        options.UserId = player.UserId;

        var alreadyExists = await _playerRepository.GetByUserIdAsync(player.UserId, token);

        if (alreadyExists != null) throw new Exception("User already exists");
        

        // Convert Player model to PlayerEntity
        var playerEntity = player.ToEntity();
        // Add to repository and return the created entity
        var createdEntity = await _playerRepository.AddAsync(playerEntity, token);
        return createdEntity.FromEntity(); // Assuming AddAsync returns PlayerEntity
    }

    public async Task<IEnumerable<Player>> GetAllPlayersAsync(GetAllPlayersOptions options, CancellationToken token)
    {
        // Get all players from the repository
        var playerEntities = await _playerRepository.GetAllAsync(options, token);
        // Convert each PlayerEntity to Player model
        var players = playerEntities.Select(playerEntity => playerEntity.FromEntity());
        return players;
    }

    public async Task<Player?> GetPlayerByIdAsync(Guid userId, CancellationToken token)
    {
        // Get the player entity from the repository
        var playerEntity = await _playerRepository.GetByUserIdAsync(userId, token);

        
        // Convert to Player model if found
        return playerEntity != null ? playerEntity.FromEntity() : throw new KeyNotFoundException($"Player with id {userId} not found");
    }


}
