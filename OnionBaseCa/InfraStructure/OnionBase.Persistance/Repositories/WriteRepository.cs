using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OnionBase.Application.Repositories;
using OnionBase.Domain.Entities.Common;
using OnionBase.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionBase.Persistance.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly UserDbContext _context;
        public WriteRepository(UserDbContext context)
        {
            _context = context;
        }
        public DbSet<T> Table => _context.Set<T>();

        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entityentry = await Table.AddAsync(model);
            return entityentry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> model)
        {
            await Table.AddRangeAsync(model);
            return true;
        }


        public bool Remove(T model)
        {
            EntityEntry<T> entityentry = Table.Remove(model);
            return entityentry.State == EntityState.Deleted;
        }

        public async Task<bool> RemoveAsync(string id)
        {
            T model = await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
            return Remove(model);

        }

        public bool Update(T model)
        {
            EntityEntry entityentry = Table.Update(model);
            return entityentry.State == EntityState.Modified;
        }
        public async Task<int> SaveAsync() =>
            await _context.SaveChangesAsync();
    }
}
