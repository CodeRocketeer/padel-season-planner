using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Requests.Team
{
    public class CreateTeamRequest
    {

        public required Guid MatchId { get; init; }

        public required Guid Player1Id { get; init; }

        public required Guid Player2Id { get; init; }



    }
}
