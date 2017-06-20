using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Web.Identity
{
    public class IdentityUser : IUser<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool LockoutEnabled { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public int AccessFailedCount { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<IdentityUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }







        //        public virtual string Email { get; set; }
        //        public virtual bool EmailConfirmed { get; set; }
        //        public virtual string PasswordHash { get; set; }
        //        public virtual string SecurityStamp { get; set; }
        //        public virtual string PhoneNumber { get; set; }
        //        public virtual bool PhoneNumberConfirmed { get; set; }
        //        public virtual bool TwoFactorEnabled { get; set; }
        //        public virtual DateTime? LockoutEndDateUtc { get; set; }
        //        public virtual bool LockoutEnabled { get; set; }
        //        public virtual int AccessFailedCount { get; set; }
    }
}