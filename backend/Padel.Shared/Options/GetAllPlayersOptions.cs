using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Shared.Options;

public class GetAllPlayersOptions
{

    public int? SeasonId { get; set; }

    public Guid? UserId { get; set; }
}
