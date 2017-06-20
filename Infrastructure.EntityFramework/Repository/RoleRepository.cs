using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using Core.Repositories;

namespace Infrastructure.EntityFramework.Repository
{
    internal class RoleRepository : Repository<Role>, IRoleRepository
    {
        internal RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Role FindByName(string roleName)
        {
            return Set.FirstOrDefault(x => x.Name == roleName);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            return Set.FirstOrDefaultAsync(x => x.Name == roleName);
        }

        public Task<Role> FindByNameAsync(CancellationToken cancellationToken, string roleName)
        {
            return Set.FirstOrDefaultAsync(x => x.Name == roleName, cancellationToken);
        }
    }
}
