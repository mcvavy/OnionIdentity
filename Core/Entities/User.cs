using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class User
    {
        public virtual int UserId { get; set; }
        public virtual string UserName { get; set; }
        //public virtual string FirstName { get; set; }
        //public virtual string LastName { get; set; }
        //public virtual DateTime? DateOfBirth { get; set; }
        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual DateTime? LockoutEndDateUtc { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual int AccessFailedCount { get; set; }

        #region Navigation Properties
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public virtual ICollection<Claim> Claims { get; set; } = new List<Claim>();
        public virtual ICollection<ExternalLogin> Logins { get; set; } = new List<ExternalLogin>();
        #endregion
    }
}
