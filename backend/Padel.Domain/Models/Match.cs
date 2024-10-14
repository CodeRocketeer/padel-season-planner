using System;
using System.Collections.Generic;

namespace Padel.Domain.Models
{
    public partial class Match
    {
        public required Guid Id { get; set; }
        public required Guid SeasonId { get; set; } // This should match 'season_id' in the database
        public required DateTime MatchDate { get; set; }

    }
}
