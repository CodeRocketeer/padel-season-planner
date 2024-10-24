using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Services.Interfaces
{
    public interface IMatchService
    {
        Task<IEnumerable<Match>> CreateBalancedMatchesForSeasonAsync(IEnumerable<Team> teams, Season season, IEnumerable<Participant> participants, CancellationToken token);
    }
}
