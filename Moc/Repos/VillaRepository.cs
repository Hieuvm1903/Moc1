using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moc.Data;
using Moc.Entities;

namespace Moc.Repos
{
    public class VillaRepository :
IVillaRepository
    {
        private readonly ApplicationContext ac;
        public VillaRepository(ApplicationContext ac)
        {
            this.ac = ac;
        }

        public async Task CreateAsync(Villa entity)
        {
            await ac.Villas.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = ac.Villas;
            if (!tracked)
            {
                query = query.AsNoTracking();

            }
            if (filter != null)
            {
                query = query.Where(filter);

            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null)
        {
            IQueryable<Villa> query = ac.Villas;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }


        public async Task RemoveAsync(Villa entity)
        {
            ac.Villas.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await ac.SaveChangesAsync();
        }

        public async Task UpdateAsync(Villa entity)
        {
            ac.Villas.Update(entity);
            await SaveAsync();
        }
    }
}
