using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelContracts.Requests.Seeder;
    public class SeedParticipantsRequest
    {
        public  int Count { get; init; }

        public  Guid SeasonId { get; init; }


    }

