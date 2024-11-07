using Padel.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Padel.Shared.Options;

namespace Padel.Infrastructure.Repositories.Interfaces;

public interface IPlayerRepository
{
    Task<IEnumerable<PlayerEntity>> GetAllAsync(GetAllPlayersOptions options, CancellationToken token = default);
    Task<PlayerEntity?> GetByUserIdAsync(Guid userId, CancellationToken token = default);
    Task<PlayerEntity> AddAsync(PlayerEntity playerEntity,CancellationToken token = default);

}