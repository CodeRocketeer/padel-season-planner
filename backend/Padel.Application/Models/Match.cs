using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Models
{
    public class Match
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public DateTime MatchDate { get; set; }

        // Constructor allowing teams to be created with IDs only
        public Match(Guid team1Id, Guid team2Id, DateTime matchDate)
        {
            Team1 = new Team { Id = team1Id }; // Create Team with ID only
            Team2 = new Team { Id = team2Id }; // Create Team with ID only
            MatchDate = matchDate;
        }

        // Constructor for a match with full teams
        public Match(Team team1, Team team2)
        {
            Team1 = team1;
            Team2 = team2;
            MatchDate = DateTime.Now; // Set to a default or pass as parameter
        }

        // Method to check if match is valid (teams belong to the same season)
        public bool IsMatchValid()
        {
            return Team1.SeasonId == Team2.SeasonId && MatchDate >= DateTime.Now;
        }
    }
}
