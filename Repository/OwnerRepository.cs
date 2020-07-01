using Entities;
using Entities.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(RepositoryContext repositoryContext)
            :base(repositoryContext)
        {
        }

        public async Task<IEnumerable<Owner>> GetAllOwnersAsync() {

            return await FindAll().OrderBy(ow=>ow.Name).ToListAsync();

        }

        public async Task<Owner> GetOwnerByIdAsync(Guid ownerId) {
            return await FindByCondition(owner=>owner.Id.Equals(ownerId)).FirstOrDefaultAsync();
        }
        /*IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        { 
            return RepositoryContext.Set<T>().Where(expression).AsNoTracking();*/

        public async Task<Owner> GetOwnerWithDetailsAsync(Guid ownerId) {
            return await FindByCondition(owner=>owner.Id.Equals(ownerId)).Include(ac=>ac.Accounts).FirstOrDefaultAsync();
        }

        public async Task CreateOwnerAsync(Owner owner) {
            Create(owner);
            await RepositoryContext.SaveChangesAsync();
        }

        public async Task UpdateOwnerAsync(Owner owner) {
            Update(owner);
            await RepositoryContext.SaveChangesAsync();
        }

        public async Task DeleteOwnerAsync(Owner owner) {
            Delete(owner);
            await RepositoryContext.SaveChangesAsync();
        }



    }
}