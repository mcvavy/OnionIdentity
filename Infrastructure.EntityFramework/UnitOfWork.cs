using System.Threading;
using System.Threading.Tasks;
using Core.Repositories;
using Infrastructure.EntityFramework.Repository;

namespace Infrastructure.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IExternalLoginRepository _externalLoginRepository;
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;

        public UnitOfWork()
        {
            _context = new ApplicationDbContext();
        }

        public IExternalLoginRepository ExternalLoginRepository => _externalLoginRepository ?? 
            (_externalLoginRepository = new ExternalLoginRepository(_context));

        public IRoleRepository RoleRepository => _roleRepository ?? (_roleRepository = new RoleRepository(_context));

        public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(_context));

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        #region IDisposable Members
        public void Dispose()
        {
            _externalLoginRepository = null;
            _roleRepository = null;
            _userRepository = null;
            _context.Dispose();
        }
        #endregion
    }
}
