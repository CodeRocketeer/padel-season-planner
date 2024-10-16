using System;
using System.Collections.Generic;

namespace Padel.Infrastructure.Entities
{
    public class TeamEntity
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }

        public Guid Player1Id { get; set; }

        public Guid Player2Id { get; set; }
        public List<PlayerEntity> Players { get; set; } = new();
    }
}
