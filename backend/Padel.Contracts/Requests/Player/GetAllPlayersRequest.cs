using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelContracts.Requests.Player
{
    public class GetAllPlayersRequest
    {
        public required int? SeasonId { get; init; }
    }
}
