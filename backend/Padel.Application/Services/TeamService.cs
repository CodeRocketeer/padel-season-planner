using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Padel.Domain.Models;
using Padel.Application.Repositories;

namespace Padel.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<bool> CreateAsync(Team team, CancellationToken token = default)
        {
            // Team validation occurs in the Team model
            return await _teamRepository.CreateAsync(team, token);
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
        {
            return _teamRepository.DeleteByIdAsync(id, token);
        }

        public Task<IEnumerable<Team>> GetAllAsync(CancellationToken token = default)
        {
            return _teamRepository.GetAllAsync(token);
        }

        public Task<Team?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _teamRepository.GetByIdAsync(id, token);
        }

        public async Task<Team?> UpdateAsync(Team team, CancellationToken token = default)
        {
            // Team validation occurs in the Team model
            var teamExists = await _teamRepository.ExistsByIdAsync(team.Id,token);
            if (!teamExists) return null;

            await _teamRepository.UpdateAsync(team, token);
            return team;
        }
    }
}
