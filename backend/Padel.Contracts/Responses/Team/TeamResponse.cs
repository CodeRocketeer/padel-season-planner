using Padel.Contracts.Responses.Participants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelContracts.Responses.Team
{
    public class TeamResponse
    {
        public required Guid Id { get; init; }              // Team ID
        public required Guid SeasonId { get; init; }        // Season the team belongs to
        public ParticipantResponse? Participant1 { get; init; } // First participant in the team
        public ParticipantResponse? Participant2 { get; init; } // Second participant in the team
    }
}
