using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Requests.Match
{
    public class MatchUpdateRequest
    {
        public Guid SeasonId { get; init; } // Link to the season
        public DateTime MatchDate { get; init; } // Scheduled match date

        public required Guid Team1Id { get; init; } // Link to team 1

        public required Guid Team2Id { get; init; } // Link to team 2
    }
}
