using Padel.Contracts.Requests.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Responses.Match
{
    public class MatchesResponse
    {
        public required IEnumerable<MatchResponse> Items { get; set; } = Enumerable.Empty<MatchResponse>();
    }
}
