using System;
using System.Collections.Generic;

namespace Padel.Infrastructure.Entities
{
    public class MatchEntity
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }
        public DateTime Date { get; set; }

        public Guid Team1Id { get; set; }
        public Guid Team2Id { get; set; }
        public List<TeamEntity> Teams { get; set; } = new();
    }
}
