using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Padel.Domain.Models;

namespace Padel.Application.Services.Interfaces;

public interface ITeamService
{


    Task<IEnumerable<Team>> GenerateAllTeamCombinations(List<Player> players);




}
