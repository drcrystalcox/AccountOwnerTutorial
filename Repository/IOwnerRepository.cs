using System;
using System.Collections.Generic;
using Entities.Models;
using System.Threading.Tasks;

namespace Repository
{
    public interface IOwnerRepository : IRepositoryBase<Owner>
    {
        Task<IEnumerable<Owner>> GetAllOwnersAsync();
        Task<Owner> GetOwnerByIdAsync(Guid ownerId);
        Task<Owner> GetOwnerWithDetailsAsync(Guid ownerId);

        Task CreateOwnerAsync(Owner owner);
        Task UpdateOwnerAsync(Owner owner);
        Task DeleteOwnerAsync(Owner owner);

    }
}