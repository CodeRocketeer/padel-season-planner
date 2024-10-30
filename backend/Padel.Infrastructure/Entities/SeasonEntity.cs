using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Padel.Infrastructure.Entities;

public class SeasonEntity
{
    [Key]
    public int Id { get; init; }

    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public int AmountOfMatches { get; set; }
    public DayOfWeek DayOfWeek { get; set; }

    // Navigation Properties
    public ICollection<MatchEntity> Matches { get; set; } = new List<MatchEntity>();

    // Many-to-Many relationship with Players
    public ICollection<PlayerEntity> Players { get; set; } = new List<PlayerEntity>();


}
