using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Responses.Team
{
    public class TeamsResponse
    {
        public required IEnumerable<TeamResponse> Items { get; set; } = Enumerable.Empty<TeamResponse>();
    }

}







