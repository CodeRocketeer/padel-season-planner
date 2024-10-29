using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padel.Application.Database.Entities;

public class TeamEntity
{
    [Key]
    public int Id { get; init; }

    // Foreign Key for Match
    public int MatchId { get; set; }
    public MatchEntity Match { get; set; }

    // Navigation Property for Players
    public ICollection<PlayerEntity> Players { get; set; } = new List<PlayerEntity>();
}
