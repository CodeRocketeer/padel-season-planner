using Padel.Infrastructure.Entities;
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
        };
    }

    public static Player FromEntity(this PlayerEntity playerEntity)
    {
        // Map entity to model, including Id
        return new Player(playerEntity.Gender, playerEntity.Name, playerEntity.UserId);
     
    }
}