using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Requests.Match
{
    public class MatchCreateRequest
    {
        public Guid SeasonId { get; init; } // Link to the season
        public DateTime MatchDate { get; init; } // Scheduled match date

        
    }
}
