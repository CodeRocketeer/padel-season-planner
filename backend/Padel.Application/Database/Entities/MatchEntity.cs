using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Padel.Application.Database.Entities;

public class MatchEntity
{
    [Key]
    public int  Id { get; init; }

    public DateTime MatchDate { get; set; }

    // Foreign Key for Season
    public int SeasonId { get; set; }
    public SeasonEntity Season { get; set; }

    // Navigation Properties
    public ICollection<TeamEntity> Teams { get; set; } = new List<TeamEntity>();
}
