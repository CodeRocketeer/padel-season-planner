using System;
using System.Collections.Generic;

namespace Padel.Infrastructure.Entities
{
    public class SeasonEntity
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public int DayOfWeek { get; set; }
        public string? Name { get; set; }
        public int AmountOfMatches { get; set; }
        public List<MatchEntity> Matches { get; set; } = new();
    }
}
