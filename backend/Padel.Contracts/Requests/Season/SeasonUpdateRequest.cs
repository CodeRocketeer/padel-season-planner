using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Requests.Season
{
    public  class SeasonUpdateRequest
    {

     
        public required  int DayOfWeek { get; init; }

        public required int AmountOfMatches { get; init; }

        public required DateTime StartDate { get; init; }

        public required string Name { get; init; }
    }
}
