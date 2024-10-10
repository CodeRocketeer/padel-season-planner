using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Requests.Match
{
    public class MatchUpdateRequest
    {
        public Guid Id { get; set; } // Unique identifier of the match
        public Guid SeasonId { get; set; } // Link to the season
        public List<Guid> TeamIds { get; set; } // List of team IDs participating in the match
        public DateTime MatchDate { get; set; } // Scheduled match date
    }
}
