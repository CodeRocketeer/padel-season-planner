using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Requests.Player
{
    public class PlayerUpdateRequest
    {

        public string Name { get; init; }
        public Guid UserId { get; init; }
        public string Sex { get; init; }

        public Guid SeasonId { get; init; }
    }
}
