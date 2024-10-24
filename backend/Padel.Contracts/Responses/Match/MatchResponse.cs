using PadelContracts.Responses.Team;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelContracts.Responses.Match
{
    public class MatchResponse
    {
        public required Guid Id { get; init; }              // Unique Match ID
        public required Guid SeasonId { get; init; }        // Season the match belongs to
        public TeamResponse? Team1 { get; init; }   // Details of Team 1
        public TeamResponse? Team2 { get; init; }   // Details of Team 2
        public DateTime MatchDate { get; init; }   // Date and time of the match
    }
}


