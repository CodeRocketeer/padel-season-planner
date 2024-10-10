using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contract.Responses.Season
{
    public class SeasonResponse
    {

        public Guid Id { get; init; }

        public required DayOfWeek DayOfWeek { get; init; }

        public required int AmountOfMatches { get; init; }

        public required DateTime StartDate { get; init; }

        public required string Name { get; init; }

        




    }
}
