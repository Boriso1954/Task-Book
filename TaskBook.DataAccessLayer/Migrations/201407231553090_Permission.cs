namespace TaskBook.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Permission : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 16),
                        Description = c.String(nullable: false, maxLength: 128),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PermissionRoles",
                c => new
                    {
                        PermissionId = c.Long(nullable: false),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.PermissionId, t.RoleId })
                .ForeignKey("dbo.Permissions", t => t.PermissionId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.PermissionId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PermissionRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PermissionRoles", "PermissionId", "dbo.Permissions");
            DropIndex("dbo.PermissionRoles", new[] { "RoleId" });
            DropIndex("dbo.PermissionRoles", new[] { "PermissionId" });
            DropTable("dbo.PermissionRoles");
            DropTable("dbo.Permissions");
        }
    }
}
