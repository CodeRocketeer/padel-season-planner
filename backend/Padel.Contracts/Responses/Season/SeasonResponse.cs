using Padel.Contracts.Responses.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Responses.Season
{
    public class SeasonResponse
    {

        public Guid Id { get; init; }

        public required int DayOfWeek { get; init; }

        public required int AmountOfMatches { get; init; }

        public required DateTime StartDate { get; init; }

        public DateTime EndDate { get; set; }

        public required string? Name { get; init; }

        public List<MatchResponse>? Matches { get; set; }

        




    }
}
