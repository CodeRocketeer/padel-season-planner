using Padel.Application.Database.Entities;

namespace Padel.Application.Models;

public class Player
{
    public int Id { get; set; }
    public Guid UserId { get; private set; }
    public Gender Gender { get; private set; }
    public string Name { get; private set; }

    public Player(Gender gender, string name, Guid userId)
    {
        // Validate Gender
        if (!Enum.IsDefined(typeof(Gender), gender))
        {
            throw new ArgumentOutOfRangeException(nameof(gender), "Invalid gender value.");
        }

        // Validate Name
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), "Name cannot be null or empty.");
        }

        // Validate UserId
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId cannot be an empty GUID.", nameof(userId));
        }


        Gender = gender;
        Name = name;
        UserId = userId;
    }

    public static PlayerEntity ToEntity(Player player)
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


    public static Player FromEntity(PlayerEntity playerEntity)
    {
        // Map entity to model, including Id
        return new Player(playerEntity.Gender, playerEntity.Name, playerEntity.UserId)
        {
            Id = playerEntity.Id,
        };
    }
}
