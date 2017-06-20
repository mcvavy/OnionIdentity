using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using Core.Entities;
using Infrastructure.EntityFramework.EntityConfiguration;

namespace Infrastructure.EntityFramework
{
    public class ApplicationDbContext : DbContext
    {
        internal IDbSet<User> Users { get; set; }
        internal IDbSet<Role> Roles { get; set; }
        internal IDbSet<ExternalLogin> Logins { get; set; }
        internal bool RequireUniqueEmail { get; set; }

        public ApplicationDbContext(): base("Name=EFIdentity")
        {
            
        }

//        public ApplicationDbContext(string nameOrConnectionString)
//            : base(nameOrConnectionString)
//        {
//            
//        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Assembly);

            base.OnModelCreating(modelBuilder);
        } 

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            if (entityEntry != null && entityEntry.State == EntityState.Added)
            {
                var errors = new List<DbValidationError>();
                var user = entityEntry.Entity as User;
                //check for uniqueness of user name and email
                if (user != null)
                {
                    if (Users != null && Users.Any(u => String.Equals(u.UserName, user.UserName)))
                    {
                        errors.Add(new DbValidationError("User",
                            String.Format(CultureInfo.CurrentCulture, "User name {0} is already taken.", user.UserName)));
                    }

                    if (RequireUniqueEmail && Users.Any(u => String.Equals(u.Email, user.Email)))
                    {
                        errors.Add(new DbValidationError("User",
                            String.Format(CultureInfo.CurrentCulture, "Email {0} is already taken.", user.Email)));
                    }
                }
                else
                {
                    var role = entityEntry.Entity as Role;
                    //check for uniqueness of role name
                    if (role != null && Roles.Any(r => String.Equals(r.Name, role.Name)))
                    {
                        errors.Add(new DbValidationError("Role",
                            String.Format(CultureInfo.CurrentCulture, "Role {0} already exists.", role.Name)));
                    }
                }
                if (errors.Any())
                {
                    return new DbEntityValidationResult(entityEntry, errors);
                }
            }
            return base.ValidateEntity(entityEntry, items);
        }
    }
}
