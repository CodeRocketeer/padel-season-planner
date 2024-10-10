using Padel.Contracts.Requests.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Responses.Player
{
    public class PlayersResponse
    {
        public required IEnumerable<PlayerResponse> Items { get; set; } = Enumerable.Empty<PlayerResponse>();
    }
}
