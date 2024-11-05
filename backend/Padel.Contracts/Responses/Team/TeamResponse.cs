using PadelContracts.Responses.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelContracts.Responses.Team
{
    public class TeamResponse
    {
        public int? Id { get; init; }              // Team ID      // Season the team belongs to
        public required PlayerResponse Player1{ get; init; } // First participant in the team
        public required PlayerResponse Player2 { get; init; } // Second participant in the team
    }
}
