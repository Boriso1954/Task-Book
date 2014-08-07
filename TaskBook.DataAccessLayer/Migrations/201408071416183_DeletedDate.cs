namespace TaskBook.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedDate : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PermissionRoles", name: "UserID", newName: "RoleID");
            RenameIndex(table: "dbo.PermissionRoles", name: "IX_UserID", newName: "IX_RoleID");
            AddColumn("dbo.Projects", "DeletedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.AspNetUsers", "DeletedDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DeletedDate");
            DropColumn("dbo.Projects", "DeletedDate");
            RenameIndex(table: "dbo.PermissionRoles", name: "IX_RoleID", newName: "IX_UserID");
            RenameColumn(table: "dbo.PermissionRoles", name: "RoleID", newName: "UserID");
        }
    }
}
