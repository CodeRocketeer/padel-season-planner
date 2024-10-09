using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Models
{
    public class Team
    {
        public required Guid Id { get; init; }

        public required Guid MatchId { get; set; }

        public required Guid Player1Id { get; set; }

        public required Guid Player2Id { get; set; }

    }
}
