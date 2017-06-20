using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Entities = Core.Entities;
using Core.Repositories;
using Microsoft.AspNet.Identity;

namespace Web.Identity
{
    public class UserStore : IUserLoginStore<IdentityUser, int>,
        IUserClaimStore<IdentityUser, int>,
        IUserPasswordStore<IdentityUser, int>,
        IUserRoleStore<IdentityUser, int>,
        IUserSecurityStampStore<IdentityUser, int>,
        IUserEmailStore<IdentityUser, int>,
        IUserTwoFactorStore<IdentityUser, int>,
        IUserLockoutStore<IdentityUser, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserStore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region IUserStore<IdentityUser, int> Members

        public Task CreateAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _unitOfWork.UserRepository.Add(Mapper.Map<Entities.User>(user));

            return _unitOfWork.SaveChangesAsync();
        }

        public Task DeleteAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _unitOfWork.UserRepository.Remove(Mapper.Map<Entities.User>(user));
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IdentityUser> FindByIdAsync(int userId)
        {
            var user = _unitOfWork.UserRepository.FindById(userId);
            return Task.FromResult<IdentityUser>(Mapper.Map<IdentityUser>(user));
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            var user = _unitOfWork.UserRepository.FindByUserName(userName);
            return Task.FromResult<IdentityUser>(Mapper.Map<IdentityUser>(user));
        }

        public Task UpdateAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentException("user");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            _unitOfWork.UserRepository.Update(Mapper.Map<Entities.User>(user));
            return _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region IUserClaimStore<IdentityUser, int> Members
        public Task<IList<Claim>> GetClaimsAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            return Task.FromResult<IList<Claim>>(u.Claims.Select(x => new Claim(x.ClaimType, x.ClaimValue)).ToList());
        }

        public Task RemoveClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var c = u.Claims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            u.Claims.Remove(c);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task AddClaimAsync(IdentityUser user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (claim == null)
                throw new ArgumentNullException(nameof(claim));

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var c = new Entities.Claim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                User = u
            };
            u.Claims.Add(c);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }
        #endregion

        #region IUserLoginStore<IdentityUser, int> Members
        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (login == null)
                throw new ArgumentNullException(nameof(login));

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var l = new Entities.ExternalLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                User = u
            };
            u.Logins.Add(l);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException(nameof(login));

            var identityUser = default(IdentityUser);

            var l = _unitOfWork.ExternalLoginRepository.GetByProviderAndKey(login.LoginProvider, login.ProviderKey);
            if (l != null)
                identityUser = Mapper.Map<IdentityUser>(l.User);

            return Task.FromResult<IdentityUser>(identityUser);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            return Task.FromResult<IList<UserLoginInfo>>(u.Logins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey)).ToList());
        }

        public Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (login == null)
                throw new ArgumentNullException(nameof(login));

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var l = u.Logins.FirstOrDefault(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);
            u.Logins.Remove(l);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region IUserRoleStore<IdentityUser, int> Members
        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: roleName.");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));
            var r = _unitOfWork.RoleRepository.FindByName(roleName);
            if (r == null)
                throw new ArgumentException("roleName does not correspond to a Role entity.", nameof(roleName));

            u.Roles.Add(r);
            _unitOfWork.UserRepository.Update(u);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            return Task.FromResult<IList<string>>(u.Roles.Select(x => x.Name).ToList());
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            return Task.FromResult<bool>(u.Roles.Any(x => x.Name == roleName));
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Argument cannot be null, empty, or whitespace: role.");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            if (u == null)
                throw new ArgumentException("IdentityUser does not correspond to a User entity.", nameof(user));

            var r = u.Roles.FirstOrDefault(x => x.Name == roleName);
            u.Roles.Remove(r);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region IUserPasswordStore<IdentityUser, int> Members

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult<bool>(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        #endregion

        #region IUserSecurityStampStore<IdentityUser, int> Members

        public Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return Task.FromResult<string>(user.SecurityStamp);
        }

        #endregion

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #region IEmailStore<IdentityUser, int>

        public Task SetEmailAsync(IdentityUser user, string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var userInDb = _unitOfWork.UserRepository.FindByUserName(user.UserName);
            if (userInDb.EmailConfirmed)
            {
                user.EmailConfirmed = true;
            }
            else
            {
                user.EmailConfirmed = false;
            }

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> FindByEmailAsync(string email)
        {
            return Task.FromResult(Mapper.Map<IdentityUser>(_unitOfWork.UserRepository.FindByUserName(email)));
        }

        #endregion

        #region IUserTwoFactorStore<IdentityUser, int>

        public Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.TwoFactorEnabled = _unitOfWork.UserRepository.FindByUserName(user.UserName).TwoFactorEnabled;
            return Task.FromResult(user.TwoFactorEnabled);
        }

        #endregion

        #region IUserLockoutStore<IdentityUser, int>

        public Task<DateTimeOffset> GetLockoutEndDateAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return
                Task.FromResult(_unitOfWork.UserRepository.FindByUserName(user.UserName).LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var dbUser = _unitOfWork.UserRepository.FindByUserName(user.UserName);

            dbUser.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? (DateTime?)null : lockoutEnd.UtcDateTime;
            _unitOfWork.SaveChanges();
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var userDb = _unitOfWork.UserRepository.FindByUserName(user.UserName);
            userDb.AccessFailedCount++;
            user.AccessFailedCount = userDb.AccessFailedCount;
            _unitOfWork.SaveChanges();
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _unitOfWork.UserRepository.FindByUserName(user.UserName).AccessFailedCount = 0;
            _unitOfWork.SaveChanges();
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userdb = _unitOfWork.UserRepository.FindByUserName(user.UserName);
            user.AccessFailedCount = userdb.AccessFailedCount;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var dbUser = _unitOfWork.UserRepository.FindByUserName(user.UserName);
            dbUser.LockoutEnabled = true;
            _unitOfWork.SaveChanges();
            return Task.FromResult(dbUser.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(IdentityUser user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        #endregion
    }
}