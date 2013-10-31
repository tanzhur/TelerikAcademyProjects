namespace ManagementSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Owner_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Plan_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Plans", t => t.Plan_Id)
                .Index(t => t.Plan_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserManagement",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        DisableSignIn = c.Boolean(nullable: false),
                        LastSignInTimeUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Todoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Priority = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        Deadline = c.DateTime(nullable: false),
                        Assignee_Id = c.String(maxLength: 128),
                        Assignor_Id = c.String(maxLength: 128),
                        Plan_Id = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Assignee_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Assignor_Id)
                .ForeignKey("dbo.Plans", t => t.Plan_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.Assignee_Id)
                .Index(t => t.Assignor_Id)
                .Index(t => t.Plan_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                        ValidUntilUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserSecrets",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 128),
                        Secret = c.String(),
                    })
                .PrimaryKey(t => t.UserName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Plan_Id", "dbo.Plans");
            DropForeignKey("dbo.Plans", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Todoes", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Todoes", "Plan_Id", "dbo.Plans");
            DropForeignKey("dbo.Todoes", "Assignor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Todoes", "Assignee_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Plans", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserManagement", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Plan_Id" });
            DropIndex("dbo.Plans", new[] { "Owner_Id" });
            DropIndex("dbo.Todoes", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Todoes", new[] { "Plan_Id" });
            DropIndex("dbo.Todoes", new[] { "Assignor_Id" });
            DropIndex("dbo.Todoes", new[] { "Assignee_Id" });
            DropIndex("dbo.Plans", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserManagement", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropTable("dbo.AspNetUserSecrets");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetTokens");
            DropTable("dbo.Todoes");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserManagement");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Plans");
        }
    }
}
