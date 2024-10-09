using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Contracts.Requests.Player
{
    public class CreatePlayerRequest
    {

        public required Guid UserId { get; set; }

        

    }
}
