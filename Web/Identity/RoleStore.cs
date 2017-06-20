using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Repositories;
using Microsoft.AspNet.Identity;

namespace Web.Identity
{
    public class RoleStore : IRoleStore<IdentityRole, int>, IQueryableRoleStore<IdentityRole, int>, IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleStore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            // Dispose does nothing since we want SimpleInjector to manage the lifecycle of our Unit of Work
        }

        public Task CreateAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));


            var r = Mapper.Map<Role>(role);

            _unitOfWork.RoleRepository.Add(r);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task UpdateAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var r = Mapper.Map<Role>(role);

            _unitOfWork.RoleRepository.Update(r);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task DeleteAsync(IdentityRole role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var r = Mapper.Map<Role>(role);

            _unitOfWork.RoleRepository.Remove(r);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IdentityRole> FindByIdAsync(int roleId)
        {
            var role = _unitOfWork.RoleRepository.FindById(roleId);
            return Task.FromResult<IdentityRole>(Mapper.Map<IdentityRole>(role));
        }



        public Task<IdentityRole> FindByNameAsync(string roleName)
        {
            var role = _unitOfWork.RoleRepository.FindByName(roleName);
            return Task.FromResult<IdentityRole>(Mapper.Map<IdentityRole>(role));
        }

        public IQueryable<IdentityRole> Roles => _unitOfWork.RoleRepository.GetAll().Select(x => Mapper.Map<IdentityRole>(x)).AsQueryable();



        //private Role getRole(IdentityRole identityRole)
        //{
        //    if (identityRole == null)
        //        return null;
        //    return new Role
        //    {
        //        RoleId = identityRole.Id,
        //        Name = identityRole.Name
        //    };
        //}


        //private IdentityRole getIdentityRole(Role role)
        //{
        //    if (role == null)
        //        return null;
        //    return new IdentityRole
        //    {
        //        Id = role.RoleId,
        //        Name = role.Name
        //    };
        //}
    }
}