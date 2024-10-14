using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Requests.Team
{
    public class TeamUpdateRequest
    {

       
        public Guid SeasonId { get; init; }
        public Guid Player1Id { get; init; }
        public Guid Player2Id { get; init; }

      


    }
}
