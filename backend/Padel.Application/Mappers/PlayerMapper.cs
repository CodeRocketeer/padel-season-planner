using Padel.Application.Database.Entities;
using Padel.Domain.Models;

namespace Padel.Application.Mappers;

public static class PlayerMapper
{
    public static PlayerEntity ToEntity(this Player player)
    {
        // Only map Id if it's not zero or default, typically for updates
        return new PlayerEntity
        {
            UserId = player.UserId,
            Name = player.Name,
            Gender = player.Gender,

            // Set Id only if it's already assigned to avoid overriding EF's auto-generation
            Id = player.Id != 0 ? player.Id : 0 // Alternatively, you could omit this line if 0 is default
        };
    }

    public static Player FromEntity(this PlayerEntity playerEntity)
    {
        // Map entity to model, including Id
        return new Player(playerEntity.Gender, playerEntity.Name, playerEntity.UserId)
        {
            Id = playerEntity.Id,
        };
    }
}