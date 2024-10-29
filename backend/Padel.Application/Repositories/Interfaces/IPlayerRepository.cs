using Padel.Application.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Application.Repositories.Interfaces;

public interface IPlayerRepository
{
    Task<PlayerEntity?> GetByIdAsync(int id, CancellationToken token = default);
    Task<IEnumerable<PlayerEntity>> GetAllAsync(CancellationToken token = default);
    Task<PlayerEntity> AddAsync(PlayerEntity playerEntity,CancellationToken token = default);
    Task<PlayerEntity> UpdateAsync(PlayerEntity playerEntity, CancellationToken token = default);
    Task<bool> DeleteAsync(int id, CancellationToken token = default);
}