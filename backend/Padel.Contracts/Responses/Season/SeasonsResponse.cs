using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Responses.Season
{
    public class SeasonsResponse
    {
        public required IEnumerable<SeasonResponse> Items { get; set; } = Enumerable.Empty<SeasonResponse>();
    }

}







