using Padel.Domain.Models;

namespace Padel.Application.Services.Interfaces;

public interface ITeamService
{


    Task<IEnumerable<Team>> GenerateAllTeamCombinations(List<Player> players);




}
