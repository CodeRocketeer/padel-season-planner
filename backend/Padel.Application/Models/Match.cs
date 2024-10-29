using Padel.Application.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Models
{
    public class Match
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }
        public Team Team1 { get; private set; }
        public Team Team2 { get; private set; }
        public DateTime MatchDate { get; set; }

        // Constructor allowing teams to be created with IDs only
        public Match(Team team1, Team team2)
        {
            
            Team1 = team1 ?? throw new ArgumentNullException(nameof(team1));
            Team2 = team2 ?? throw new ArgumentNullException(nameof(team2));

            if (team1.Id == team2.Id)
            {
                throw new ArgumentException("Team1 and Team2 cannot have the same ID.", nameof(team1));
            }


            // Check for common players based on UserId
            if (team1.Player1.UserId == team2.Player1.UserId ||
                team1.Player1.UserId == team2.Player2.UserId ||
                team1.Player2.UserId == team2.Player1.UserId ||
                team1.Player2.UserId == team2.Player2.UserId)
            {
                throw new ArgumentException("Teams cannot have common players.");
            }



        }


        // Convert Match model to MatchEntity
        public static MatchEntity ToEntity(Match match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            return new MatchEntity
            {
                Id = match.Id != 0 ? match.Id : 0,
                SeasonId = match.SeasonId,
                MatchDate = match.MatchDate
                // Teams can be handled separately if needed
            };
        }

        // Convert MatchEntity back to Match model
        public static Match FromEntity(MatchEntity matchEntity, Team team1, Team team2)
        {
            if (matchEntity == null)
                throw new ArgumentNullException(nameof(matchEntity));

            if (team1 == null || team2 == null)
                throw new ArgumentNullException("Teams cannot be null");

            return new Match(team1, team2) // Corrected to use team2
            {
                Id = matchEntity.Id,
                SeasonId = matchEntity.SeasonId,
                MatchDate = matchEntity.MatchDate
            };
        }

    }
}
