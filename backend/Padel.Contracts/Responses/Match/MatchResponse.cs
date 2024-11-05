using Padel.Domain.Models;
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
        public required int Id { get; init; }              // Unique Match ID
        public TeamResponse? Team1 { get; init; }   // Details of Team 1
        public TeamResponse? Team2 { get; init; }   // Details of Team 2
        public DateTime MatchDate { get; init; }   // Date and time of the match
    }
}


