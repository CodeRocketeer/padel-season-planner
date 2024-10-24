using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Models
{
    public class Team
    {
        public Guid Id { get; set; }
        public Guid SeasonId { get; set; }
        public Participant Participant1 { get; set; }
        public Participant Participant2 { get; set; }

        // Constructor allowing optional participants
        public Team(Participant player1 = null, Participant player2 = null)
        {
            Participant1 = player1;
            Participant2 = player2;
        }

        // Overloaded constructor for creating a team with just IDs
        public Team(Guid id, Guid seasonId)
        {
            Id = id;
            SeasonId = seasonId;
            Participant1 = null; // No participants initially
            Participant2 = null; // No participants initially
        }
        // Method to check if both players are valid and part of the same season
        public bool AreParticipantsValidForSeason(Guid seasonId)
        {
            return Participant1.SeasonId == seasonId && Participant2.SeasonId == seasonId;
        }

        public List<Participant> GetParticipants() => new List<Participant> { Participant1, Participant2 }; // <--->
    }

}
