using FluentValidation;
using Padel.Application.Models;
using Padel.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Services
{
    public class TeamService : ITeamService
    {

        private readonly ITeamRepository _teamRepository;
        private readonly IValidator<Team> _teamValidator;

        public TeamService(ITeamRepository teamRepository, IValidator<Team> teamValidator)
        {
            _teamRepository = teamRepository;
            _teamValidator = teamValidator;
        }

        public async  Task<bool> CreateAsync(Team team)
        {
            await _teamValidator.ValidateAndThrowAsync(team);
            return await _teamRepository.CreateAsync(team);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            return _teamRepository.DeleteByIdAsync(id);
        }

        public Task<IEnumerable<Team>> GetAllAsync()
        {
            return _teamRepository.GetAllAsync();
        }

        public Task<Team?> GetByIdAsync(Guid id)
        {
            return _teamRepository.GetByIdAsync(id);
        }

        public async Task<Team?> UpdateAsync(Team team)
        {   
            await _teamValidator.ValidateAndThrowAsync(team);
            var teamExists = await _teamRepository.ExistsByIdAsync(team.Id);
            if (!teamExists) return null;

            await _teamRepository.UpdateAsync(team);

            return team;
        }
    }
}
