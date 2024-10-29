using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Services.Interfaces;

public interface IMatchService
{


    Task<IEnumerable<Match>> GenerateAllMatchCombinations(List<Team> teams);




}
