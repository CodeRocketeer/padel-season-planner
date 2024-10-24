using Padel.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Services.Interfaces
{
    public interface ISeederService
    {
        Task<bool> CreateManyParticipantsAsync(int count, Guid seasonId, CancellationToken token = default);


    }
}
