using Padel.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padel.Infrastructure.Entities;

public class PlayerEntity
{
    [Key]
    public int Id { get; init; }

    public Guid UserId { get; set; }
    public string Name { get; set; }
    public Gender Gender { get; set; }

    // Many-to-Many relationship with Teams
    public ICollection<TeamEntity> Teams { get; set; } = new List<TeamEntity>();

    // Many-to-Many relationship with Seasons
    public ICollection<SeasonEntity> Seasons { get; set; } = new List<SeasonEntity>();

}
