using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Infrastructure.EntityFramework.EntityConfiguration
{
    internal class UserConfiguration : EntityTypeConfiguration<User>
    {
        internal UserConfiguration()
        {
            ToTable("User");

            HasKey(x => x.UserId)
                .Property(x => x.UserId)
                .HasColumnName("UserId")
                //.HasColumnType("uniqueidentifier")
                .IsRequired();

            Property(x => x.SecurityStamp)
                .HasColumnName("SecurityStamp")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            //Property(x => x.FirstName)
            //    .HasColumnName("FirstName")
            //    .HasColumnType("nvarchar")
            //    .HasMaxLength(256)
            //    .IsRequired();

            //Property(x => x.LastName)
            //    .HasColumnName("LastName")
            //    .HasColumnType("nvarchar")
            //    .HasMaxLength(256)
            //    .IsRequired();


            Property(x => x.UserName)
                .HasColumnName("UserName")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();

            HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .Map(x =>
                {
                    x.ToTable("UserRole");
                    x.MapLeftKey("UserId");
                    x.MapRightKey("RoleId");
                });

            HasMany(x => x.Claims)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId);


            HasMany(x => x.Logins)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}
