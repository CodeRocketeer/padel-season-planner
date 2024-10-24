using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Services.Interfaces
{
    public interface IParticipantService
    {
        Task<bool> ParticipateInSeasonAsync(Participant participant, CancellationToken token = default);

        Task<bool> LeaveSeasonAsync(Guid seasonId, Guid userId, CancellationToken token = default);

        Task<IEnumerable<Participant>> GetAllAsync(GetAllParticipantsOptions options, CancellationToken token = default);


    }
}
