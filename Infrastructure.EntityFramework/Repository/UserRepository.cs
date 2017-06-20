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
    internal class UserRepository : Repository<User>, IUserRepository
    {
        internal UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public User FindByUserName(string username)
        {
            return Set.FirstOrDefault(x => x.UserName == username);
        }

        public Task<User> FindByUserNameAsync(string username)
        {
            return Set.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public Task<User> FindByUserNameAsync(CancellationToken cancellationToken, string username)
        {
            return Set.FirstOrDefaultAsync(x => x.UserName == username, cancellationToken);
        }
    }
}
