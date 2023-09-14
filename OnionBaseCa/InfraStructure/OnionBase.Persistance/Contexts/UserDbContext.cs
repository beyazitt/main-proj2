using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnionBase.Domain.Entities;
using OnionBase.Domain.Entities.Common;
using OnionBase.Domain.Entities.Identity;

namespace OnionBase.Persistance.Contexts
{
    public class UserDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> UserDatas { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Question>  Questions { get; set; }
        public DbSet<Campaigns> Campaigns { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker
                .Entries<BaseEntity>();
            foreach(var data in datas)
            {
                _ = data.State switch
                {
                    EntityState.Modified => data.Entity.UpdatedTime = DateTime.UtcNow,
                    EntityState.Deleted => data.Entity.UpdatedTime = DateTime.UtcNow,
                    EntityState.Added => data.Entity.CreatedTime = DateTime.UtcNow,
                    EntityState.Unchanged => data.Entity.UpdatedTime = DateTime.UtcNow
                };
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
