namespace Infrastructure.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExternalLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(maxLength: 4000),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Claim",
                c => new
                    {
                        ClaimId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(maxLength: 4000),
                        ClaimValue = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.ClaimId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExternalLogin", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Claim", "UserId", "dbo.User");
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.Claim", new[] { "UserId" });
            DropIndex("dbo.ExternalLogin", new[] { "UserId" });
            DropTable("dbo.UserRole");
            DropTable("dbo.Role");
            DropTable("dbo.Claim");
            DropTable("dbo.User");
            DropTable("dbo.ExternalLogin");
        }
    }
}
